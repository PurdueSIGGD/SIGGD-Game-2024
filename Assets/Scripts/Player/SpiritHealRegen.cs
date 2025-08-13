using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHealRegen : MonoBehaviour
{
    [SerializeField] float healPerSpirit;
    [SerializeField] HealingContext healContext;

    private void OnEnable()
    {
        Spirit.SpiritCollected += OnSpiritPicked;
    }

    private void OnDisable()
    {
        Spirit.SpiritCollected -= OnSpiritPicked;
    }

    void OnSpiritPicked(Spirit.SpiritType spirit)
    {
        healContext.healing = healPerSpirit;
        GetComponent<Health>().Heal(healContext, gameObject);
    }
}
