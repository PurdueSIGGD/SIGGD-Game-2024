using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageIce : EnemyStateManager
{
    [SerializeField] GameObject icePositionsParent;
    [SerializeField] GameObject iceShardPrefab;
    [SerializeField] float iceAttackIntervalSec;
    [SerializeField] GameObject lineOfSightTriggerBox;

    private List<GameObject> iceShards;



    [Header("Basic Projectile")]
    [SerializeField] private GameObject projectile;
    private Vector3 throwPosition = Vector3.zero;

    public void StartBasic()
    {
        throwPosition = PlayerID.instance.transform.position;
        if (!IsCurrentTargetPlayer())
        {
            throwPosition = GetCurrentTarget().transform.position;
        }
    }

    public void FireProjectile()
    {
        Instantiate(projectile, (transform.position + (0.5f * Vector3.right)), transform.rotation).GetComponent<EnemyProjectile>().Init(gameObject, throwPosition);
    }



    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnMageStunned;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnMageStunned;
    }

    void Update()
    {
        // manually flip the mage to face the target
        GameObject target = player.gameObject;
        if (!IsCurrentTargetPlayer())
        {
            target = GetCurrentTarget();
        }
        if (target != null)
        {
            if (target.transform.position.x - transform.position.x < 0)
            {
                Flip(false);
            }
            else
            {
                Flip(true);
            }
        }
    }
    public void IceAttack()
    {
        Transform[] icePositions = icePositionsParent.GetComponentsInChildren<Transform>();
        StartCoroutine(IceAttackCoroutine(icePositions));
    }
    IEnumerator IceAttackCoroutine(Transform[] icePositions)
    {
        GameObject target = player.gameObject;
        if (!IsCurrentTargetPlayer())
        {
            target = GetCurrentTarget();
        }
        iceShards = new List<GameObject>();
        foreach (Transform icePos in icePositions)
        {
            if (icePos.gameObject == icePositionsParent)
                continue;

            MageIceShardAttack ice = Instantiate(iceShardPrefab, icePos.position, Quaternion.identity).GetComponent<MageIceShardAttack>();
            ice.Initialize(target, this.gameObject);
            iceShards.Add(ice.gameObject);
            yield return new WaitForSeconds(iceAttackIntervalSec);
        }
    }
    void OnDestroy()
    {
        CancelChargeUp();
        StopAllCoroutines();
    }
    /*
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(lineOfSightTriggerBox.transform.position, lineOfSightTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }
    */

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, lineOfSightTriggerBox.transform.lossyScale.x);
    }


    public void LaunchShards()
    {
        StartCoroutine(LaunchAttackCoroutine());
    }


    private IEnumerator LaunchAttackCoroutine()
    {
        if (iceShards == null || iceShards.Count <= 0) yield break;
        foreach (GameObject iceShard in iceShards)
        {
            if (iceShard == null) continue;
            iceShard.GetComponent<MageIceShardAttack>().Launch();
            yield return new WaitForSeconds(iceAttackIntervalSec);
        }
        iceShards.Clear();
    }


    private void CancelChargeUp()
    {
        StopAllCoroutines();
        if (iceShards == null || iceShards.Count <= 0) return;
        foreach (GameObject iceShard in iceShards)
        {
            if (iceShard == null) continue;
            if (!iceShard.GetComponent<MageIceShardAttack>().launched)
            {
                Destroy(iceShard);
            }
        }
        iceShards.Clear();
    }


    public void OnMageStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        CancelChargeUp();
    }
}
