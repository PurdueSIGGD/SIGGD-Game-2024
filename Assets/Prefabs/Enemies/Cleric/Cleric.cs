using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : EnemyStateManager
{

    [Header("Staff Swing Fields")]
    [SerializeField] Transform swingContactBox;
    [SerializeField] DamageContext swingContext;
    [SerializeField] float swingDamage;

    [Header("Bubble Channel Fields")]

    [SerializeField] GameObject channelTriggerBox;
    [SerializeField] float maxChannelRadius;
    [SerializeField] GameObject bubble;
    float bubbleStandardCooldown;
    [SerializeField] float bubbleMissCooldown;

    public void Start()
    {
        base.Start();

    }

    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the cleric rather than require direct L.O.S.
        return Physics2D.OverlapCircle(channelTriggerBox.transform.position, channelTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player"));
    }

    protected void StartStaffSwing()
    {
        GenerateDamageFrame(swingContactBox.position, swingContactBox.lossyScale.x, swingContactBox.lossyScale.y, swingContext, gameObject);
    }

    protected void EndStaffSwing()
    {
        // good soup!
    }

    protected void ChannelBubble()
    {
        GameObject target = GrabRandomNearbyBubbleTarget();
        if (target != null)
        {
            ApplyBubbleAsChildTo(target);
        }
        else
        {
            ApplyBubbleAsChildTo(gameObject);
        }
    }

    private GameObject GrabRandomNearbyBubbleTarget()
    {
        Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, maxChannelRadius, LayerMask.GetMask("Enemy"));
        List<GameObject> validTargets = new List<GameObject>();
        foreach (Collider2D col in collision)
        {
            GameObject enemy = col.gameObject;
            if ((enemy.GetComponent<Knight>() != null) ||
                (enemy.GetComponent<Mage>() != null))
            {
                validTargets.Add(enemy);
            }
        }
        if (validTargets.Count == 0) return null;
        if (validTargets.Count == 1) return validTargets[0];
        int randomIndex = Random.Range(0, validTargets.Count);
        return validTargets[randomIndex];
    }

    private void ApplyBubbleAsChildTo(GameObject enemy)
    {
        if (enemy != null)
        {
            ClericBubbleShieldScript bubbleScript = Instantiate(bubble, enemy.transform).GetComponent<ClericBubbleShieldScript>();
            bubbleScript.SetParentEnemy(enemy);
        }
    }
}
