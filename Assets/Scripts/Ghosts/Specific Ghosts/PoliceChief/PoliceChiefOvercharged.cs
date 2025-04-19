using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PoliceChiefOvercharged : Skill
{
    [SerializeField] int[] pointCounts = {0, 40, 80, 120, 160};
    private int bonusDamage = 0;
    private StatManager statManager;
    private PlayerStateMachine playerStateMachine;
    private float timer = 0.0f;

    private void Start()
    {
        statManager = PlayerID.instance.GetComponent<StatManager>();
        playerStateMachine = PlayerID.instance.GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            GetComponent<Animator>().SetTrigger("ToIdle");
            Debug.Log("Explode");
        }

        if (playerStateMachine.currentAnimation.Equals("police_chief_special_primed"))
        {
            timer = 2f;
        }
    }

    public override void AddPointTrigger()
    {
        bonusDamage = pointCounts[GetPoints()];
        statManager.ModifyStat("Special Damage", bonusDamage - pointCounts[GetPoints() - 1]);
    }
    public override void RemovePointTrigger()
    {

    }
    public override void ClearPointsTrigger()
    {
        statManager.ModifyStat("Special Damage", - bonusDamage);
    }

}
