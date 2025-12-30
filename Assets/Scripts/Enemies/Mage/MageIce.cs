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

    void Update()
    {
        // manually flip the mage to face the player
        if (player != null)
        {
            if (player.position.x - transform.position.x < 0)
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
        foreach (Transform icePos in icePositions)
        {
            if (icePos.gameObject == icePositionsParent)
                continue;

            MageIceShardAttack ice = Instantiate(iceShardPrefab, icePos.position, Quaternion.identity).GetComponent<MageIceShardAttack>();
            ice.Initialize(player.gameObject, this.gameObject);
            yield return new WaitForSeconds(iceAttackIntervalSec);
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(lineOfSightTriggerBox.transform.position, lineOfSightTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player")) || base.HasLineOfSight(tracking);
    }

    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, lineOfSightTriggerBox.transform.lossyScale.x);
    }
}
