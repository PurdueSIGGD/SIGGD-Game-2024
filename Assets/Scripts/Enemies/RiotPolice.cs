using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Riot Police
/// </summary>
public class RiotPolice : EnemyStateManager
{
    [Header("Baton Attack")]
    [SerializeField] protected Transform batonTrigger;
    [SerializeField] protected DamageContext batonDamage;
    [SerializeField] GameObject batonVisual;
    bool isWindingUp = false;

    private AudioSource windupSFX = null;

    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnStunned;
        GameplayEventHolder.OnDeathFilter.Add(OnKilled);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnStunned;
        GameplayEventHolder.OnDeathFilter.Remove(OnKilled);
    }

    protected override void Start()
    {
        base.Start();
        batonDamage.damage = stats.ComputeValue("Damage");
    }

    protected void OnBatonStart()
    {
        isWindingUp = true;
        windupSFX = AudioManager.Instance.SFXBranch.PlaySFXTrack("RiotPoliceBaton");
    }

    // Check for collision in swing range to deal damage
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage, gameObject);
        batonVisual.SetActive(true);
    }

    protected void OnBatonEnd()
    {
        isWindingUp = false;
        windupSFX = null;
        batonVisual.SetActive(false);
    }

    private void CancelWindup()
    {
        if (isWindingUp)
        {
            isWindingUp = false;
            if (windupSFX != null) { windupSFX.Stop(); }
        }
    }

    public void OnStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        CancelWindup();
    }

    public void OnKilled(ref DamageContext context)
    {
        if (context.victim != gameObject) return;
        CancelWindup();
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
