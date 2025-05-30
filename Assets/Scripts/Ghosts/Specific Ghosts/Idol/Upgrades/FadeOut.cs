using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Fade out skill for Eva
 * If you want to change how the transparency setting when the player is invisible,
 * Go to Invisible.cs script and change the duration variable.
 */

public class FadeOut : Skill
{
    IdolManager idolManager;
    GameObject player;
    IdolSpecial idolSpecial;

    [SerializeField] private float invisibilityDuration = 4f;  // designers: this field is serialized, so change it 
    private static int pointIndex;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        idolManager = gameObject.GetComponent<IdolManager>();

        idolManager.evaSelectedEvent.AddListener(EvaSelected);
        idolManager.evaDeselectedEvent.AddListener(EvaDeselected);
    }


    private void EvaSelected()
    {
        if (pointIndex == 0)
        {
            return;
        }
        idolSpecial = idolManager.special;
        idolSpecial.avaliableHoloJumpVA.Add("Eva-Idol Fade Out Activate");
        idolSpecial.holoJumpCreatedCloneEvent.AddListener(HoloJumpCreatedClone);
    }

    private void EvaDeselected()
    {
        if (player.GetComponent<Invisible>() != null)
        {
            Destroy(player.GetComponent<Invisible>());
        }
    }

    private void HoloJumpCreatedClone()
    {
        if (player.GetComponent<Invisible>() == null)
        {
            player.AddComponent<Invisible>();
            GameplayEventHolder.OnDamageFilter.Add(BuffLightAttack);

            StartCoroutine(RemoveInvisibilityOnTimer(invisibilityDuration));
        }
    }

    private void BuffLightAttack(ref DamageContext damageContext)
    {
        if (damageContext.attacker.CompareTag("Player"))
        {
            // play audio
            AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Fade Out Hit");

            // buff damage
            float damageMultiplier = 1f + 0.3f * pointIndex;
            damageContext.damage *= damageMultiplier;
            StartCoroutine(RemoveInvisibilityOnTimer(0f)); // we MUST wait 1 frame before removing invis. Here's why: ask Temirlan
        }
    }

    private IEnumerator RemoveInvisibilityOnTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveInvisibility();
    }

    private void RemoveInvisibility()
    {
        Debug.Log("attempting to remove invis!");
        if (player.GetComponent<Invisible>() != null)
        {
            Destroy(player.GetComponent<Invisible>());
        }
        if (GameplayEventHolder.OnDamageFilter.Contains(BuffLightAttack))
        {
            GameplayEventHolder.OnDamageFilter.Remove(BuffLightAttack);
        }
    }


    // We don't have to do anything when points are added or removed
    // because damage multiplier calculation happens dynamically in BuffLightAttack
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
}
