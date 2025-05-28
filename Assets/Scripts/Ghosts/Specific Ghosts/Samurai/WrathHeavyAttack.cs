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
    private float timer = 0.0f;
    private bool startTimer = false;
    private bool resetDecay = false;

    // Start is called before the first frame update
    void Start()
    {
        GameplayEventHolder.OnDamageDealt += OnDamage;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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

        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                PlayerID.instance.GetComponent<Animator>().SetBool("finishWrath", true);
                startTimer = false;
            }
        }
    }

    //The function gets called (via event) whenever something gets damaged in the scene
    public void OnDamage(DamageContext context)
    {
        if (context.attacker == gameObject && context.actionTypes[0] == ActionType.LIGHT_ATTACK)
        {
            float wrathGained = manager.GetStats().ComputeValue("Wrath Percent Gain Per Damage Dealt");
            wrathPercent = Mathf.Min(wrathPercent + wrathGained, 1);
            decayTimer = manager.GetStats().ComputeValue("Wrath Decay Buffer");
            startingToDecay = true;
            decaying = false;
        }
        else if (context.victim == gameObject)
        {
            float wrathLost = manager.GetStats().ComputeValue("Wrath Percent Loss Per Damage Taken");
            wrathPercent = Mathf.Max(wrathPercent - wrathLost, 0);
        }
    }

    //this functionn get called (via message from animator) when you enter the Heavy Charge up state
    public void StartHeavyChargeUp()
    {
        decaying = false;
        resetDecay = true;
    }

    //this function get called (via message form animator) when you enter the Heavy Attack state
    public void StartHeavyAttack()
    {
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

        float dist = Mathf.Lerp(manager.GetStats().ComputeValue("Heavy Attack Minimum Travel Distance"), manager.GetStats().ComputeValue("Heavy Attack Maximum Travel Distance "), wrathPercent);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Enemy", "Ground"));
        if (hit)
        {
            dist = Mathf.Abs(transform.position.x - hit.point.x);
        }
        PlayerID.instance.GetComponent<Rigidbody2D>().velocity = manager.GetStats().ComputeValue("Heavy Attack Travel Speed") * dir;
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        timer = dist / manager.GetStats().ComputeValue("Heavy Attack Travel Speed");
        startTimer = true;
    }

    //this function get called (via message form animator) when you exit the Heavy Attack state
    public void StopHeavyAttack()
    {
        PlayerID.instance.GetComponent<Animator>().SetBool("finishWrath", false);

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
    }

    //this function get called (via message form animator) when you exit the 2nd Heavy Attack state
    public void StopSamuraiHeavyAttack()
    {
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        PlayerID.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        if (resetDecay)
        {
            decaying = true;
            resetDecay = false;
        }
    }

    //Used to empty the remaining wrath percentage
    public void ResetWrath()
    {
        wrathPercent = 0.0f;
    }

    public float GetWrathPercent()
    {
        return this.wrathPercent;
    }
}
