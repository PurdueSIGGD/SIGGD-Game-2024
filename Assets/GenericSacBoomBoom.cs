using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSacBoomBoom : Sacrifice
{
    [SerializeField] DamageContext context;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject vfx;
    [SerializeField] string track;
    [SerializeField] string afterTrack;
    [SerializeField] string windupSFX;
    [SerializeField] string explosionSFX;
    [SerializeField] float damage = 400f;
    [SerializeField] float windupTime = 1.85f;

    TimeFreezeManager timeFreezeManager;
    //GhostManager ghostManager;
    GhostIdentity ghostIdentity;

    bool isSaccin = false;
    float sacTime = 0f;



    private void Update()
    {
        if (isSaccin)
        {
            sacTime += Time.deltaTime;
            //AudioManager.Instance.SFXBranch.GetSFXTrack("RespawnInOblivionSFX").SetPitch(sacTime, windupTime);
            if (sacTime >= windupTime)
            {
                //AudioManager.Instance.SFXBranch.StopSFXTrack("RespawnInOblivionSFX");
                sacTime = 0f;
                isSaccin = false;
                UnleashSac();
            }
        }
    }



    public override void DoSac()
    {
        base.DoSac();

        AudioManager.Instance.SFXBranch.PlaySFXTrack("GenericSacrificeSFX");
        if (windupSFX != "") AudioManager.Instance.SFXBranch.PlaySFXTrack(windupSFX);

        timeFreezeManager = PlayerID.instance.GetComponent<TimeFreezeManager>();
        //ghostManager = GetComponent<GhostManager>();
        ghostIdentity = GetComponent<GhostIdentity>();
        //track = ghostManager.identityName + " Sacrifice Before";

        AudioManager.Instance.VABranch.PlayVATrack(track);
        GameObject introExplosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        introExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(5f, ghostIdentity.GetCharacterInfo().primaryColor);

        sacTime = 0f;
        isSaccin = true;
    }



    private void UnleashSac()
    {
        if (explosionSFX != "") AudioManager.Instance.SFXBranch.PlaySFXTrack(explosionSFX);

        GameObject explosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, ghostIdentity.GetCharacterInfo().primaryColor);

        context.damage = damage;
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(PlayerID.instance.transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.transform.gameObject.GetComponent<Health>().Damage(context, PlayerID.instance.gameObject);
        }

        StartCoroutine(PlayAfterLine());
    }

    private IEnumerator PlayAfterLine()
    {
        yield return new WaitForSeconds(1.5f);
        if (afterTrack != "") AudioManager.Instance.VABranch.PlayVATrack(afterTrack);
    }



    IEnumerator Explode()
    {
        AudioManager.Instance.VABranch.PlayVATrack(track);
        GameObject introExplosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        introExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(3f, ghostIdentity.GetCharacterInfo().primaryColor);

        yield return new WaitForSeconds(1.85f);

        GameObject explosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, ghostIdentity.GetCharacterInfo().primaryColor);

        //yield return new WaitForSeconds(0.1f);

        //GameObject badExplosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        //badExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, Color.red);

        context.damage = damage;

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(PlayerID.instance.transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.transform.gameObject.GetComponent<Health>().Damage(context, PlayerID.instance.gameObject);
        }
    }


    IEnumerator FreezeTime()
    {
        if (timeFreezeManager)
        {
            timeFreezeManager.FreezeTime(1);

            yield return new WaitForSeconds(1);

            timeFreezeManager.UnFreezeTime();
        }
    }
}
