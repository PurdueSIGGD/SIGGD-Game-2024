using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class WrathHeavyAttack : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    [SerializeField] private DamageContext dashDamageContext;
    [SerializeField] private LayerMask attackMask;

    private float wrathPercent = 0.0f;
    private StatManager stats;
    private Camera mainCamera;

    private float decayTimer = 0.0f;
    private bool startingToDecay = false;
    private bool decaying = false;
    private float timer = 0.0f;
    private bool startTimer = false;
    private float dashSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameplayEventHolder.OnDamageDealt += OnDamage;
        stats = PlayerID.instance.GetComponent<StatManager>();
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

        if (decaying && wrathPercent >= stats.ComputeValue("Wrath Decay Rate") * Time.deltaTime)
        {
            wrathPercent -= stats.ComputeValue("Wrath Decay Rate") * Time.deltaTime;
        }
        else
        {
            wrathPercent = 0.0f;
        }

        Debug.Log("Time delta: " + Time.deltaTime);

        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                PlayerID.instance.GetComponent<Move>().PlayerGo();
                PlayerID.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                PlayerID.instance.GetComponent<Animator>().SetBool("finishWrath", true);
                startTimer = false;
            }
        }
    }

    public float GetWrath()
    {
        return wrathPercent;
    }

    public void OnDamage(DamageContext context)
    {
        if (context.attacker == gameObject && context.actionTypes[0] == ActionType.LIGHT_ATTACK)
        {
            wrathPercent = Mathf.Min(wrathPercent + stats.ComputeValue("Wrath Gained"), 1);
            decayTimer = stats.ComputeValue("Wrath Decay Buffer");
            startingToDecay = true;
            decaying = false;
            Debug.Log("Wrath Percent Now: " +  wrathPercent);
        }
        else if (context.victim == gameObject)
        {
            wrathPercent = Mathf.Max(wrathPercent - stats.ComputeValue("Wrath Gained"), 0);
        }
    }

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

        float dist = Mathf.Lerp(stats.ComputeValue("Base Dash Distance"), stats.ComputeValue("Max Dash Distance"), wrathPercent);
        Debug.Log("Wrath percent: " + wrathPercent);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, attackMask);
        if (hit)
        {
            dist = transform.position.x - hit.point.x;
        }
        PlayerID.instance.GetComponent<Rigidbody2D>().velocity = stats.ComputeValue("Dash Speed") * dir;
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        timer = dist / stats.ComputeValue("Dash Speed");
        startTimer = true;
        Empty();
    }

    public void StopHeavyAttack()
    {
        PlayerID.instance.GetComponent<Move>().PlayerGo();
        PlayerID.instance.GetComponent<Animator>().SetBool("finishWrath", false);

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 1), 0, attackMask);
        foreach (Collider2D h in hit)
        {
            IDamageable damageable = h.gameObject.GetComponent<IDamageable>();
            dashDamageContext.damage = stats.ComputeValue("Dash Damage");
            if (damageable != null)
            {
                damageable.Damage(dashDamageContext, gameObject);
            }
        }
    }

    public void Empty()
    {
        wrathPercent = 0.0f;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
