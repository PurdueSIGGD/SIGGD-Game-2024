using UnityEngine;

public class PoliceChiefOvercharged : Skill
{
    [SerializeField] float[] pointCounts = {1.4f, 1.8f, 2.2f, 2.6f};
    [SerializeField] float explosionRadius = 50f;
    [SerializeField] LayerMask attackmask;
    [SerializeField] DamageContext explosionContext;
    [SerializeField] float damageToEnemy;
    [SerializeField] float damageToPlayer;
    [SerializeField] GameObject circleVFX;
    private float bonusDamage = 1.0f;
    private StatManager statManager;
    private PlayerStateMachine playerStateMachine;
    private float timer = -1.0f;
    private bool reset = true;

    private void Start()
    {
        playerStateMachine = PlayerID.instance.gameObject.GetComponent<PlayerStateMachine>();
        if (GetPoints() > 0)
        {
            bonusDamage = pointCounts[GetPoints() - 1];
        }
        else{
            bonusDamage = 1.0f;
        }
        Debug.Log("Overcharged bonus dmg: " +  bonusDamage);
    }

    private void Update()
    {

        if (timer > 0.0f && bonusDamage != 1.0f) {
            timer -= Time.deltaTime;
            if (timer < 0.0f) {
                explosionContext.damage = damageToPlayer;
                PlayerID.instance.gameObject.GetComponent<Health>().Damage(explosionContext, PlayerID.instance.gameObject);
                PlayerID.instance.gameObject.GetComponent<Animator>().SetTrigger("toIdle");
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, attackmask);
                explosionContext.damage = damageToEnemy;
                foreach (Collider2D collider in colliders)
                {
                    collider.gameObject.GetComponent<Health>().Damage(explosionContext, PlayerID.instance.gameObject);
                }
                GameObject circlevfx = Instantiate(circleVFX);
                circlevfx.transform.position = PlayerID.instance.gameObject.transform.position;
                circlevfx.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, Color.red);
                //Debug.Log("Explode");
            }
        }

        if (playerStateMachine.currentAnimation.Equals("police_chief_special_primed") && timer < 0.0f && reset)
        {
            timer = 2f;
            reset = false;
        }

        if (bonusDamage != 1.0f && playerStateMachine.currentAnimation.Equals("player_idle"))
        {
            reset = true;
            timer = -1f;
        }
    }

    public override void AddPointTrigger()
    {
        bonusDamage = pointCounts[GetPoints() - 1];
    }

    public override void RemovePointTrigger()
    {

    }

    public override void ClearPointsTrigger()
    {
        bonusDamage = 1.0f;
    }

    void OnDamaged(DamageContext context)
    {
        if(context.actionID == ActionID.POLICE_CHIEF_SPECIAL)
        {
            context.damage *= Mathf.Lerp(1, bonusDamage, (timer) / 2.0f);
        }
    }

}
