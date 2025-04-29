using UnityEngine;

public class OrionManager : GhostManager
{
    [SerializeField] public GameObject heavyIndicator;
    [SerializeField] public DamageContext heavyDamage;
    [SerializeField] public float offsetX;

    [HideInInspector] public bool isDashEnabled = true;
    [HideInInspector] public bool isAirbornePostDash = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        heavyDamage.damage = stats.ComputeValue("Heavy Damage");
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
