using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadientWell : Skill
{
    [SerializeField] GameObject wellObj;
    private GameObject wellInstance;

    void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += SummonWell;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= SummonWell;
    }

    private void SummonWell(ActionContext context)
    {
        if (GetPoints() <= 0 || context.actionID != ActionID.KING_SPECIAL || (context.extraContext != null && !context.extraContext.Equals("Activated")))
        {
            return;
        }

        //Destroy old well
        if (wellInstance != null)
        {
            wellInstance.GetComponent<RadientWellEffect>().EndEffect();
        }

        Vector2 playerPos = PlayerID.instance.transform.position;

        // raycast down to find floor, summon well there
        RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit)
        {
            wellInstance = Instantiate(wellObj, hit.point, transform.rotation);
            RadientWellEffect wellEffect = wellInstance.GetComponent<RadientWellEffect>();
            wellEffect.Init(GetPoints());
        }
        else // if no floor (how?), then I guess summon at player location
        {
            wellInstance = Instantiate(wellObj, playerPos, transform.rotation);
            RadientWellEffect wellEffect = wellInstance.GetComponent<RadientWellEffect>();
            wellEffect.Init(GetPoints());
        }

        // SFX
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Radiant Well");
    }
    
    public override void AddPointTrigger() { }

    public override void ClearPointsTrigger() { }

    public override void RemovePointTrigger() { }
}
