using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cleric : EnemyStateManager
{

    [Header("Staff Swing Fields")]
    [SerializeField] Transform swingContactBox;
    [SerializeField] DamageContext swingContext;
    [SerializeField] float swingDamage;
    [SerializeField] GameObject swingVisual;
    bool isSwinging;

    [Header("Bubble Channel Fields")]

    [SerializeField] GameObject channelTriggerBox;
    [SerializeField] float maxChannelRadius;
    [SerializeField] GameObject bubble;
    float bubbleStandardCooldown;
    [SerializeField] float bubbleMissCooldown;
    private LineRenderer lineRenderer;

    private GameObject bubbleInstance;


    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += OnClericDead;
        GameplayEventHolder.OnEntityStunned += OnClericStunned;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= OnClericDead;
        GameplayEventHolder.OnEntityStunned -= OnClericStunned;
    }


    public void Start()
    {
        base.Start();
        swingContext.damage = swingDamage;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void Update()
    {
        // manually flip the cleric to face the player
        if (!isSwinging)
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

        if (bubbleInstance == null || bubbleInstance.GetComponent<ClericBubbleShieldScript>().isEnding)
        {
            lineRenderer.enabled = false;
            return;
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position + new Vector3(((player.position.x - transform.position.x < 0) ? 0.38f : -0.38f), 0.7f, 0f));
        Vector3 endPosition = bubbleInstance.transform.position;
        Vector3 dir = Vector3.Normalize(transform.position - bubbleInstance.transform.position);
        endPosition += (1.2f * dir);
        lineRenderer.SetPosition(1, endPosition);
    }

    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the cleric rather than require direct L.O.S.
        return Physics2D.OverlapCircle(channelTriggerBox.transform.position, channelTriggerBox.transform.lossyScale.x, LayerMask.GetMask("Player"));
    }

    protected void EnterSwingAnimation()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninSwordWindup");
        isSwinging = true;
    }

    protected void StartStaffSwing()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("ClericMelee");
        swingVisual.SetActive(true);
        GenerateDamageFrame(swingContactBox.position, swingContactBox.lossyScale.x, swingContactBox.lossyScale.y, swingContext, gameObject);
    }

    protected void EndStaffSwing()
    {
        // good soup!
        swingVisual.SetActive(false);
        isSwinging = false;
    }

    protected void ChannelBubble()
    {
        if (bubbleInstance != null) return;
        GameObject target = GrabRandomNearbyBubbleTarget();
        if (target != null)
        {
            ApplyBubbleAsChildTo(target);
        }
        /*
        else
        {
            ApplyBubbleAsChildTo(gameObject);
        }
        */
    }

    private GameObject GrabRandomNearbyBubbleTarget()
    {
        Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, maxChannelRadius, LayerMask.GetMask("Enemy"));
        List<GameObject> validTargets = new List<GameObject>();
        foreach (Collider2D col in collision)
        {
            GameObject enemy = col.gameObject;
            if ((enemy.GetComponentInChildren<ClericBubbleShieldScript>() == null) &&
                !((enemy.GetComponent<Cleric>() != null) ||
                 (enemy.GetComponent<Crow>() != null)))
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
            ClericBubbleShieldScript oldBubble = enemy.GetComponentInChildren<ClericBubbleShieldScript>();
            if (oldBubble != null)
            {
                Destroy(oldBubble.gameObject);
            }
            else
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("ClericShieldUp");
            }
            //ClericBubbleShieldScript bubbleScript = Instantiate(bubble, enemy.transform).GetComponent<ClericBubbleShieldScript>();
            bubbleInstance = Instantiate(bubble, enemy.transform);
            ClericBubbleShieldScript bubbleScript = bubbleInstance.GetComponent<ClericBubbleShieldScript>();
            bubbleScript.SetParentEnemy(enemy);
        }
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(swingContactBox.position, swingContactBox.lossyScale);
    }





    private void CancelBubble()
    {
        if (bubbleInstance == null) return;
        bubbleInstance.GetComponent<ClericBubbleShieldScript>().EndBubble();
    }



    public void OnClericStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        CancelBubble();
    }



    public void OnClericDead(DamageContext context)
    {
        if (context.victim != gameObject) return;
        CancelBubble();
    }
}
