//#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Relentless Fury skill for Akihito
/// </summary>
public class RelentlessFury : Skill
{

    private SamuraiManager samuraiManager;

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 0.15f, 0.25f, 0.35f, 0.45f
    };
    private int pointIndex;

    [SerializeField] public int parryStacksGained = 3;
    [SerializeField] public int maxStacks = 3;
    [HideInInspector] public int buffStacks = 0;





    private void Start()
    {
        samuraiManager = gameObject.GetComponent<SamuraiManager>();
#if DEBUG_LOG
        Debug.Log("Relentless Fury enabled");
#endif

    }

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += HandleBoost;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= HandleBoost;
        GameplayEventHolder.OnDamageFilter.Remove(BuffLightAttack);
#if DEBUG_LOG
        Debug.Log("Relentless Fury disabled");
#endif
    }

    private void Update()
    {

    }





    private void HandleBoost(ActionContext context)
    {
        if (samuraiManager.selected && pointIndex > 0 &&
            context.actionID == ActionID.SAMURAI_SPECIAL &&
            context.extraContext.Equals("Parry Success"))
        {
            if (!GameplayEventHolder.OnDamageFilter.Contains(BuffLightAttack))
            {
                GameplayEventHolder.OnDamageFilter.Add(BuffLightAttack);
            }
            buffStacks = Mathf.Min(buffStacks + parryStacksGained, maxStacks);
        }
    }



    private void BuffLightAttack(ref DamageContext damageContext)
    {
        if (damageContext.attacker.CompareTag("Player") &&
            (damageContext.actionTypes.Contains(ActionType.LIGHT_ATTACK) &&
             !damageContext.actionTypes.Contains(ActionType.SKILL)))
        {
            // play audio
            //AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Fade Out Hit");

            // buff damage
            damageContext.damage += values[pointIndex];
            damageContext.ghostID = GhostID.AKIHITO;
            buffStacks--;
            if (buffStacks <= 0)
            {
                StartCoroutine(RemoveBuffOnTimer(0f));
            }
        }
    }



    private IEnumerator RemoveBuffOnTimer(float duration)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(duration);
        buffStacks = 0;
        if (GameplayEventHolder.OnDamageFilter.Contains(BuffLightAttack))
        {
            GameplayEventHolder.OnDamageFilter.Remove(BuffLightAttack);
        }
    }





    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();

    }

}
