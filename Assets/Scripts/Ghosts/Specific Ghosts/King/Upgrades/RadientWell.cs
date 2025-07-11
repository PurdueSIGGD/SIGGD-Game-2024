using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadientWell : Skill
{
    [SerializeField] GameObject wellObj;

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
        if (GetPoints() <= 0 || context.actionID != ActionID.KING_SPECIAL || !context.extraContext.Equals("Activated"))
        {
            return;
        }
        Vector2 playerPos = PlayerID.instance.transform.position;

        // raycast down to find floor, summon well there
        RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit)
        {
            RadientWellEffect wellEffect = Instantiate(wellObj, hit.point, transform.rotation).GetComponent<RadientWellEffect>();
            wellEffect.Init(GetPoints());
        }
        else // if no floor (how?), then I guess summon at player location
        {
            Instantiate(wellObj, playerPos, transform.rotation);
        }
    }
    
    public override void AddPointTrigger() { }

    public override void ClearPointsTrigger() { }

    public override void RemovePointTrigger() { }
}
