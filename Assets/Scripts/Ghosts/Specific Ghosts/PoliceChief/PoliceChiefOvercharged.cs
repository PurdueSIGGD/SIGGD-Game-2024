using UnityEngine;

public class PoliceChiefOvercharged : Skill
{
    /*
    [SerializeField] float[] pointCounts = {0f, 40f, 80f, 120f, 160f};
    [SerializeField] float explosionRadius = 50f;
    [SerializeField] LayerMask attackmask;
    //[SerializeField] DamageContext explosionContext;
    [SerializeField] float damageToEnemy;
    [SerializeField] float damageToPlayer;
    [SerializeField] GameObject circleVFX;
    private float bonusDamage = 1.0f;
    private StatManager statManager;
    //private PlayerStateMachine playerStateMachine;
    //private float timer = -1.0f;
    private bool reset = true;
    */

    [SerializeField] private float[] values = { 0f, 40f, 80f, 120f, 160f };
    [HideInInspector] public int pointIndex;
    [SerializeField] public float overchargeDuration = 2f;
    [SerializeField] private float minDamage = 10f;
    [SerializeField] private float minExplosionRadius = 3f;
    [SerializeField] private float maxExplosionRadius = 8f;
    [SerializeField] DamageContext explosionContext;
    [SerializeField] private float playerBaseDamage = 10f;
    [SerializeField] private float playerDamagePercent = 0.25f;

    private PlayerStateMachine playerStateMachine;
    private PoliceChiefManager manager;
    [HideInInspector] public float timer = -1.0f;
    [HideInInspector] public bool isOvercharging = false;



    private void Start()
    {
        playerStateMachine = PlayerID.instance.gameObject.GetComponent<PlayerStateMachine>();
        manager = GetComponent<PoliceChiefManager>();
        /*
        if (GetPoints() > 0)
        {
            bonusDamage = pointCounts[GetPoints() - 1];
        }
        else{
            bonusDamage = 1.0f;
        }
        Debug.Log("Overcharged bonus dmg: " +  bonusDamage);
        */
    }

    private void Update()
    {

        /*
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
        */

        /*
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
        */

        if (isOvercharging && timer > 0f && pointIndex > 0)
        {
            AudioManager.Instance.SFXBranch.GetSFXTrack("North-Railgun Primed Loop").SetPitch((overchargeDuration - timer), overchargeDuration);
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Explode Player's PeePee
                AudioManager.Instance.SFXBranch.GetSFXTrack("North-Railgun Primed Loop").SetPitch(0f, 1f);
                timer = 0f;
                MisfireRailgun();
            }
        }
    }

    public override void AddPointTrigger()
    {
        //bonusDamage = pointCounts[GetPoints()];
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        //bonusDamage = pointCounts[GetPoints()];
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        //bonusDamage = pointCounts[GetPoints()];
        pointIndex = GetPoints();
    }

    /*
    void OnDamaged(DamageContext context)
    {
        if(context.actionID == ActionID.POLICE_CHIEF_SPECIAL)
        {
            context.damage *= Mathf.Lerp(1, bonusDamage, (timer) / 2.0f);
        }
    }
    */



    public void StartOvercharging()
    {
        timer = overchargeDuration;
        isOvercharging = true;
    }

    public void StopOvercharging()
    {
        isOvercharging = false;
    }

    public DamageContext GetExplosionDamage()
    {
        timer = Mathf.Clamp(timer, 0f, overchargeDuration);
        explosionContext.damage = Mathf.Lerp(minDamage, values[pointIndex], (overchargeDuration - timer) / overchargeDuration);
        return explosionContext;
    }

    public DamageContext GetExplosionDamage(bool selfDamage)
    {
        if (!selfDamage) return GetExplosionDamage();
        timer = Mathf.Clamp(timer, 0f, overchargeDuration);
        explosionContext.damage = playerBaseDamage + (values[pointIndex] * playerDamagePercent);
        return explosionContext;
    }

    public float GetExplosionRadius()
    {
        timer = Mathf.Clamp(timer, 0f, overchargeDuration);
        return Mathf.Lerp(minExplosionRadius, maxExplosionRadius, (overchargeDuration - timer) / overchargeDuration);
    }

    private void MisfireRailgun()
    {
        if (pointIndex <= 0) return;

        // Damage enemies
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(PlayerID.instance.transform.position, GetExplosionRadius(), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.transform.gameObject.GetComponent<Health>().Damage(GetExplosionDamage(), PlayerID.instance.gameObject);
        }

        // Damage player
        PlayerID.instance.gameObject.GetComponent<Health>().Damage(GetExplosionDamage(true), PlayerID.instance.gameObject);
        playerStateMachine.SetStun(0.4f);

        // Explosion VFX
        GameObject explosion = Instantiate(manager.specialImpactExplosionVFX, PlayerID.instance.transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(GetExplosionRadius(), manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Misfire");

        playerStateMachine.EnableTrigger("OPT");
        StopOvercharging();
        manager.special.KillSpecial();
    }
}
