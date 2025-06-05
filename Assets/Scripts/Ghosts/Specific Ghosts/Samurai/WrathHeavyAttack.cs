using UnityEngine;

/// <summary>
/// Thsi is the script that manages the "Wrath" value and activates the Samurai Heavy Attack Dash
/// </summary>
public class WrathHeavyAttack : MonoBehaviour
{
    public SamuraiManager manager;

    [SerializeField] private float wrathPercent = 0.0f;
    private Camera mainCamera;

    private float decayTimer = 0.0f;
    private bool startingToDecay = false;
    private bool decaying = false;
    private bool resetDecay = false;

    private PlayerStateMachine psm;
    private bool isCharging = false;
    private float chargingTime = 0f;
    private bool isPrimed = false;
    private float primedTime = 0f;

    private Rigidbody2D rb;
    private Vector2 desiredDashVelocity;
    private float desiredDashDist;
    private Vector2 desiredDashDest;
    private bool isDashing;
    private Vector2 startingDashLocation;

    // Start is called before the first frame update
    void Start()
    {
        GameplayEventHolder.OnDamageDealt += OnDamage;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        psm = GetComponent<PlayerStateMachine>();
        rb = PlayerID.instance.GetComponent<Rigidbody2D>();
    }

    void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (startingToDecay)
        {
            decayTimer -= Time.deltaTime;
            if (decayTimer < 0.0f)
            {
                startingToDecay = false;
                decaying = true;
            }
        }
        float decayRate = manager.GetStats().ComputeValue("Wrath Decay Rate");
        if (decaying && wrathPercent >=  decayRate * Time.deltaTime)
        {
            wrathPercent -= decayRate * Time.deltaTime;
        }
        else if (decaying)
        {
            wrathPercent = 0.0f;
            decaying = false;
        }

        if (isDashing)
        {
            rb.velocity = desiredDashVelocity;
            if (Mathf.Sign(transform.position.x - desiredDashDest.x) == Mathf.Sign(desiredDashVelocity.x))
            {
                psm.EnableTrigger("finishWrath");
                StopSamuraiHeavyAttack();
                isDashing = false;
            }
        }

        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) psm.EnableTrigger("OPT");

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;
        if (isPrimed && primedTime <= 0f) psm.EnableTrigger("OPT");

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = transform.position - mousePos;
        if (isCharging || isPrimed)
        {
            if (mouseDiff.x < 0) // update player facing direction
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (mouseDiff.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    //The function gets called (via event) whenever something gets damaged in the scene
    public void OnDamage(DamageContext context)
    {
        if (context.attacker == gameObject && context.actionTypes[0] == ActionType.LIGHT_ATTACK)
        {
            float wrathGained = manager.GetStats().ComputeValue("Wrath Percent Gain Per Damage Dealt") * context.damage / 100;
            wrathPercent = Mathf.Min(wrathPercent + wrathGained, 1);
            decayTimer = manager.GetStats().ComputeValue("Wrath Decay Buffer");
            startingToDecay = true;
            decaying = false;
        }
        else if (context.victim == gameObject)
        {
            float wrathLost = manager.GetStats().ComputeValue("Wrath Percent Loss Per Damage Taken") * context.damage / 100;
            wrathPercent = Mathf.Max(wrathPercent - wrathLost, 0);
        }
    }

    //this functionn get called (via message from animator) when you enter the Heavy Charge up state
    public void StartHeavyChargeUp()
    {
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        chargingTime = manager.GetStats().ComputeValue("Heavy Charge Up Time");
        isCharging = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackWindUp");
        decaying = false;
        resetDecay = true;
    }
    public void StopHeavyChargeUp()
    {
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        isCharging = false;
        chargingTime = 0f;
    }

    public void StartHeavyPrimed()
    {
        GetComponent<Move>().PlayerStop();
        primedTime = manager.GetStats().ComputeValue("Heavy Primed Autofire Time");
        isPrimed = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackPrimed");
    }

    public void StopHeavyPrimed()
    {
        GetComponent<Move>().PlayerGo();
        isPrimed = false;
        primedTime = 0f;
    }

    //this function get called (via message form animator) when you enter the Heavy Attack state
    public void StartHeavyAttack() { }

    public void ExecuteHeavyAttack()
    {
        GetComponent<PlayerParticles>().PlayHeavyAttackVFX();
        psm.SetLightAttack2Ready(false);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttack");

        Vector2 dir = Vector2.zero;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < PlayerID.instance.transform.position.x)
        {
            dir = new Vector2(-1, 0);
        }
        else
        {
            dir = new Vector2(1, 0);
        }

        startingDashLocation = transform.position;
        desiredDashDist = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Travel Distance"), manager.GetStats().ComputeValue("Heavy Attack Maximum Travel Distance"), wrathPercent);
        desiredDashDest = new(transform.position.x + desiredDashDist * dir.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, desiredDashDist, LayerMask.GetMask("Enemy", "Ground"));
        if (hit)
        {
            desiredDashDist = Mathf.Abs(transform.position.x - hit.point.x);
            desiredDashDest = hit.point;
        }
        desiredDashVelocity = manager.GetStats().ComputeValue("Heavy Attack Travel Speed") * dir;
        rb.isKinematic = true;
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        isDashing = true;
    }

    //this function get called (via message form animator) when you exit the Heavy Attack state
    public void StopHeavyAttack() 
    {
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 1), 0, LayerMask.GetMask("Enemy"));
        foreach (Collider2D h in hit)
        {
            Health health = h.GetComponent<Health>();
            if (health)
            {
                DamageContext context = manager.heavyDamageContext;
                context.damage = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Damage"),
                                            manager.GetStats().ComputeValue("Heavy Attack Maximum Damage"),
                                            wrathPercent);
                health.Damage(context, gameObject);
            }
        }
        ResetWrath();
        if (resetDecay)
        {
            decaying = true;
            resetDecay = false;
        }
    }

    private void StopSamuraiHeavyAttack()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    //Used to empty the remaining wrath percentage
    public void ResetWrath()
    {
        wrathPercent = 0.0f;
    }

    public float GetWrathPercent()
    {
        return wrathPercent;
    }
}
