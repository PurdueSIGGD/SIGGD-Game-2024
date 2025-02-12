using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WrathHeavyAttack : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    [SerializeField] private DamageContext dashDamageContext;

    private float wrathPercent = 0.0f;
    private StatManager stats;
    private Camera mainCamera;

    private float decayTimer = 0.0f;
    private bool startingToDecay = false;
    private bool decaying = false;
    private float timer = 0.0f;
    private bool startTimer = false;

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

        if (decaying && wrathPercent >= stats.ComputeValue("Wratch Decay Rate") * Time.deltaTime)
        {
            wrathPercent -= stats.ComputeValue("Wratch Decay Rate") * Time.deltaTime;
        }
        else
        {
            wrathPercent = 0.0f;
        }

        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {
                PlayerID.instance.GetComponent<Move>().PlayerGo();
                PlayerID.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
        }
        else if (context.victim == gameObject)
        {
            wrathPercent = Mathf.Max(wrathPercent - stats.ComputeValue("Wrath Gained"), 0);
        }
    }

    public void StartHeavyAttack()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < PlayerID.instance.transform.position.x)
        {
            PlayerID.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(-stats.ComputeValue("Dash Speed"), 0);
        }
        else
        {
            PlayerID.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(stats.ComputeValue("Dash Speed"), 0);
        }
        timer = Mathf.Lerp(0, stats.ComputeValue("Max Dash Distance"), wrathPercent) / stats.ComputeValue("Dash Speed");
        startTimer = true;
        PlayerID.instance.GetComponent<Move>().PlayerStop();
        wrathPercent = 0.0f;
    }

    public void StopHeavyAttack()
    {
        PlayerID.instance.GetComponent<Move>().PlayerGo();
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
