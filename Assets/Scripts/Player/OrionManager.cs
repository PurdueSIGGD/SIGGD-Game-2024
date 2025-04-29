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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.transform.position.y < transform.position.y)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack(SFXTrackName.LANDING);
            }
        }
    }
}
