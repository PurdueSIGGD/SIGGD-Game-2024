using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Roomba
/// </summary>
public class Roomba : EnemyStateManager
{
    [Header("Self Detonate")]
    [SerializeField] protected Transform kaboomTrigger;
    [SerializeField] protected DamageContext kaboomDamage;
    [SerializeField] float damage;
    [SerializeField] GameObject explodeMarker;
    [SerializeField] GameObject explodeVisual;
    SpriteRenderer spriteRenderer;
    bool toggleRed = true;
    bool isWindingUp = false;

    private AudioSource windupSFX = null;

    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnRoombaStunned;
        GameplayEventHolder.OnDeathFilter.Add(OnRoombaKilled);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnRoombaStunned;
        GameplayEventHolder.OnDeathFilter.Remove(OnRoombaKilled);
    }

    protected override void Start()
    {
        base.Start();
        kaboomDamage.damage = damage;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void OnInitiateKaboom()
    {
        isWindingUp = true;
        windupSFX = AudioManager.Instance.SFXBranch.PlaySFXTrack("RoombaWindup");
        StartCoroutine(flicker());
    }
    // Check for player in blast radius and do damage
    protected void OnKaboomEvent()
    {
        if (windupSFX != null) { windupSFX.Stop(); }
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoombaExplosion");
        isWindingUp = false;
        explodeMarker.SetActive(false);
        explodeVisual.SetActive(true);
        StopAllCoroutines();
        GenerateDamageFrame(kaboomTrigger.position, kaboomTrigger.lossyScale.x, kaboomDamage, gameObject);
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
        kaboomDamage.victim = this.gameObject;
        GetComponent<Health>().Damage(kaboomDamage, this.gameObject);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(kaboomTrigger.position, kaboomTrigger.lossyScale.x);
    }
    IEnumerator flicker()
    {
        while (true)
        {
            explodeMarker.SetActive(toggleRed);
            if (toggleRed)
            {
                spriteRenderer.color = Color.red;
                toggleRed = false;
            }
            else
            {
                spriteRenderer.color = Color.white;
                toggleRed = true;
            }
            yield return new WaitForSeconds(0.05f);
            yield return null;
        }
    }

    private void CancelWindup()
    {
        if (isWindingUp)
        {
            isWindingUp = false;
            if (windupSFX != null) { windupSFX.Stop(); }
            explodeMarker.SetActive(false);
            explodeVisual.SetActive(false);
            StopAllCoroutines();
        }
    }

    public void OnRoombaStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        CancelWindup();
    }

    public void OnRoombaKilled(ref DamageContext context)
    {
        if (context.victim != gameObject) return;
        CancelWindup();
    }
}
