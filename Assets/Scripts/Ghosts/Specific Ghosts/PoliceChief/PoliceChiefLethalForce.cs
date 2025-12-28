using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceChiefLethalForce : Skill
{
    [SerializeField] private int[] pointCounts = { 4, 3, 2, 1 };
    [SerializeField] private float bonusDamage;
    private int numHits = -1;
    private int consecutiveHits = 0;
    private float timer = -1.0f;
    private float recoveryTimer = -1f;
    [NonSerialized] public bool shotEmpowered = false;
    private PoliceChiefManager manager;

    LevelSwitching levelSwitchingScript;

    private void Start()
    {
        /*if(GetPoints() > 0)
        {
            numHits = pointCounts[GetPoints() - 1];
        }
        else
        {
            numHits = -1;
        }*/
        manager = GetComponent<PoliceChiefManager>();
        //AudioManager.Instance.SFXBranch.StopSFXTrack("North-Sidearm Primed Loop");

        if (numHits == -1)
        {
            SaveManager.data.north.lethalForceProgress = 0;
            return;
        }

        levelSwitchingScript = FindFirstObjectByType<LevelSwitching>();
        if (SceneManager.GetActiveScene().name.Equals(levelSwitchingScript.GetHomeWorld()))
        {
            //reservedCount = reserveCharges[pointIndex];
            //SaveManager.data.north.reserveSpecialCharges = reservedCount;
            consecutiveHits = 0;
            SaveManager.data.north.lethalForceProgress = consecutiveHits;
        }
        else
        {
            consecutiveHits = SaveManager.data.north.lethalForceProgress;
        }

        if (consecutiveHits < numHits)
        {
            AudioManager.Instance.SFXBranch.StopSFXTrack("North-Sidearm Primed Loop");
        }
        else
        {
            shotEmpowered = true;
            if (PlayerID.instance.gameObject.GetComponent<PartyManager>().GetSelectedGhost().Equals(GetComponent<GhostIdentity>()))
            {
                PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
            }
        }
    }

    private void OnEnable()
    {
        //GameplayEventHolder.OnDamageFilter.Add(OnDamage);
        GameplayEventHolder.OnDamageFilter.Insert(0, OnDamage);
        GameplayEventHolder.OnAbilityUsed += OnAbilityUse;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
        GameplayEventHolder.OnAbilityUsed -= OnAbilityUse;
    }

    private void Update()
    {
        if (timer > 0) { 
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = -1f;
                consecutiveHits = 0;
                SaveManager.data.north.lethalForceProgress = consecutiveHits;
            }
        }

        if (recoveryTimer > 0)
        {
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer <= 0)
            {
                recoveryTimer = -1f;
                consecutiveHits = 0;
                SaveManager.data.north.lethalForceProgress = consecutiveHits;
            }
        }
    }

    public override void AddPointTrigger()
    {
        numHits = pointCounts[GetPoints() - 1];
    }
    public override void RemovePointTrigger()
    {
        numHits = pointCounts[GetPoints() - 1];
    }
    public override void ClearPointsTrigger()
    {
        numHits = -1;
    }

    public void OnDamage(ref DamageContext context)
    {
        if (numHits != -1 && context.attacker == PlayerID.instance.gameObject && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            if (context.extraContext != null && context.extraContext.Equals("Power Spike Explosion")) return;
            if (consecutiveHits < numHits)
            {
                consecutiveHits += 1;
                SaveManager.data.north.lethalForceProgress = consecutiveHits;
                timer = -1.0f;
                if (consecutiveHits >= numHits)
                {
                    // Empower Shot
                    shotEmpowered = true;
                    AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Sidearm Primed Loop");
                    PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostEmpowered(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
                    AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Lethal Force");
                }
            } else if (!context.actionTypes.Contains(ActionType.SKILL))
            {
                //context.damage += bonusDamage;
                //context.damageStrength = DamageStrength.HEAVY;
                context.isCriticalHit = true;
            }
        }
    }

    public void OnAbilityUse(ActionContext action)
    {
        if (action.actionID == ActionID.POLICE_CHIEF_BASIC && !action.actionTypes.Contains(ActionType.SKILL))
        {
            if (numHits != -1 && consecutiveHits >= numHits)
            {
                shotEmpowered = false;
                timer = -1f;
                recoveryTimer = 0.1f;
                AudioManager.Instance.SFXBranch.StopSFXTrack("North-Sidearm Primed Loop");
                PlayerID.instance.GetComponent<PlayerParticles>().StopGhostEmpowered();
            }
            else
            {
                timer = 0.1f;
            }
        }
    }

    public int GetConsecutiveHits()
    {
        return consecutiveHits;
    }

    public int GetTotalHits()
    {
        return numHits;
    }
}
