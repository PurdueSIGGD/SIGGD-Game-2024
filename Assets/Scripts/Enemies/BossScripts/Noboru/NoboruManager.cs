using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoboruManager : EnemyStateManager
{

    [Header("NOBORU PARAMS")]
    NoboruController controller;
    [SerializeField] GameObject tpSource;
    List<Transform> teleportPositions = new List<Transform>();
    int tpIndex;
    [SerializeField] float visionRange;
    [SerializeField] GameObject yokaiPrefab;
    [SerializeField] int numYokai;
    [SerializeField] List<GameObject> yokaiSpawnPool = new List<GameObject>();
    [SerializeField] float fancySummonInterval;
    [SerializeField] Transform tpTriggerBox;
    [SerializeField] Transform summonTriggerSphere;

    [Header("Fireball?")]
    [SerializeField] float ballChance;
    [SerializeField] GameObject fireballPrefab;
    [Header("Lightning!!!")]
    [SerializeField] float lightningChance;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] float lightningDamage;
    [SerializeField] DamageContext lightningContext;
    [SerializeField] float lightningRadius;
    [SerializeField] float followTimeSec;
    [SerializeField] float warningTimeSec;
    [SerializeField] float lightningTimeSec;

    [SerializeField] GameObject actionPulseVFX;
    [SerializeField] Color summonPulseColor;
    [SerializeField] Color fireballPulseColor;
    [SerializeField] Color teleportPulseColor;

    public void Start()
    {
        base.Start();
        controller = GetComponent<NoboruController>();
        Transform[] tpPositions = tpSource.GetComponentsInChildren<Transform>(includeInactive: false);
        teleportPositions = new(tpPositions);
        transform.position = teleportPositions[0].position;
        tpIndex = 0;
        lightningContext.damage = lightningDamage;

    }

    public void StartWindup()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("NoboruWindup");
    }

    public void Teleport()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("NoboruTeleport");
        GameObject pulse = Instantiate(actionPulseVFX, transform.position, Quaternion.identity);
        pulse.GetComponent<RingExplosionHandler>().playRingExplosion(25f, teleportPulseColor);
        tpIndex++;
        if (tpIndex >= teleportPositions.Count)
        {
            tpIndex = 0;
        }

        Transform tpPos = teleportPositions[tpIndex];
        transform.position = tpPos.position;
    }
    void SpawnYokai()
    {
        int index = Random.Range(0, yokaiSpawnPool.Count);
        GameObject enemyToSpawn = yokaiSpawnPool[index];
        controller.SpawnYokai(yokaiPrefab, enemyToSpawn);
    }
    public void SummonYokaiWave()
    {
        //AudioManager.Instance.SFXBranch.PlaySFXTrack("NoboruSummon");
        GameObject pulse = Instantiate(actionPulseVFX, transform.position, Quaternion.identity);
        pulse.GetComponent<RingExplosionHandler>().playRingExplosion(25f, summonPulseColor);
        StartCoroutine(FancySummonCoroutine());
    }
    IEnumerator FancySummonCoroutine()
    {
        for (int i = 0; i < numYokai; i++)
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("NoboruSummon");
            SpawnYokai();
            yield return new WaitForSeconds(fancySummonInterval);
        }
    }

    public void RandomCast()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("NoboruFireball");
        GameObject pulse = Instantiate(actionPulseVFX, transform.position, Quaternion.identity);
        pulse.GetComponent<RingExplosionHandler>().playRingExplosion(25f, fireballPulseColor);
        if (PlayerID.instance != null)
        {
            float randSpell = Random.value * (ballChance + lightningChance);
            if (randSpell > ballChance)
            {
                FireLightningStuff();
            }
            else
            {
                FireFireBall();
            }
        }
    }
    public void FireFireBall()
    {
        MageFireBallAttack ball = Instantiate(fireballPrefab, transform.position, Quaternion.identity).GetComponent<MageFireBallAttack>();
        ball.Initialize(PlayerID.instance.gameObject, this.gameObject);
    }
    public void FireLightningStuff()
    {
        GameObject player = PlayerID.instance.gameObject;
        GameObject lightningObject = Instantiate(lightningPrefab, player.transform.position, Quaternion.identity);
        MageLightningAttack lightningScript = lightningObject.GetComponent<MageLightningAttack>();

        lightningScript.Initialize(PlayerID.instance.gameObject, lightningRadius, lightningContext, gameObject);
        lightningScript.StartIndependentSequence(followTimeSec, warningTimeSec, lightningTimeSec);
    }

    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(transform.position, visionRange, LayerMask.GetMask("Player"));
    }
    public void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, summonTriggerSphere.lossyScale.x);
        Gizmos.DrawWireCube(tpTriggerBox.position, tpTriggerBox.lossyScale);
    }
}
