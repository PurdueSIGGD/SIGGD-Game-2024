using System.Collections;
using UnityEngine;

public class PoliceChiefOvercharged : Skill
{
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
    }

    private void Update()
    {
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
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }



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
            manager.GetComponent<PoliceChiefEnergySiphonSkill>().ReduceCooldownOnHit();
        }

        // Damage player
        PlayerID.instance.gameObject.GetComponent<Health>().Damage(GetExplosionDamage(true), PlayerID.instance.gameObject);
        playerStateMachine.SetStun(0.4f);
        manager.GetComponent<PoliceChiefEnergySiphonSkill>().ReduceCooldownOnHit();

        // Explosion VFX
        StartCoroutine(MisfireVFX());

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Misfire");
        AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Overcharged Failure");

        playerStateMachine.EnableTrigger("OPT");
        StopOvercharging();
        manager.special.KillSpecial();
    }

    private IEnumerator MisfireVFX()
    {
        GameObject explosion = Instantiate(manager.specialImpactExplosionVFX, PlayerID.instance.transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(GetExplosionRadius(), manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        yield return new WaitForSeconds(0.1f);

        GameObject badExplosion = Instantiate(manager.specialImpactExplosionVFX, PlayerID.instance.transform.position, Quaternion.identity);
        badExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(GetExplosionRadius(), Color.red);
    }
}
