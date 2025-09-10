using System.Collections;
using UnityEngine;

public class OrionManager : GhostManager
{
    [SerializeField] public GameObject heavyIndicator;
    [SerializeField] public DamageContext heavyDamage;
    [SerializeField] public float offsetX;

    [SerializeField] public CharacterSO orionSO;

    [HideInInspector] public bool isDashEnabled = true;
    [HideInInspector] public bool isAirbornePostDash = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        heavyDamage.damage = stats.ComputeValue("Heavy Damage");
        StartCoroutine(DelayedStartCoroutine());
    }

    private IEnumerator DelayedStartCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        int cooldownSpeedBoost = Mathf.FloorToInt(PlayerID.instance.GetComponent<PlayerBuffStats>().GetStats().ComputeValue("Cooldown Speed Boost") - 100f);
        //stats.ModifyStat("Basic Cooldown", -cooldownSpeedBoost);
        stats.ModifyStat("Dash Cooldown", -cooldownSpeedBoost);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isAirbornePostDash && animator.GetBool("p_grounded"))
        {
            isAirbornePostDash = false;
        }
        isDashEnabled = (!(getSpecialCooldown() > 0f  || isAirbornePostDash));
    }
}
