using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSacBoomBoom : Sacrifice
{
    [SerializeField] DamageContext context;
    [SerializeField] float explosionRadius;
    [SerializeField] GameObject vfx;
    [SerializeField] string track;

    TimeFreezeManager timeFreezeManager;

    public override void DoSac()
    {
        base.DoSac();

        timeFreezeManager = PlayerID.instance.GetComponent<TimeFreezeManager>();
        StartCoroutine(FreezeTime());
        StartCoroutine(Explode());

        //AudioManager.Instance.VABranch.PlayVATrack(track);

        //yield return new WaitForSeconds(0.1f);

        //GameObject badExplosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        //badExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, Color.red);
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject explosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        explosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        yield return new WaitForSeconds(0.1f);

        //GameObject badExplosion = Instantiate(vfx, PlayerID.instance.transform.position, Quaternion.identity);
        //badExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(explosionRadius, Color.red);

        context.damage = 200;

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
