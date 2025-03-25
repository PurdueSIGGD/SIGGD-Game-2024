using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Start is called before the first frame update
    public interface IDamageable
    {

        /// <summary>
        /// Reduces this entity's health to a minimum of zero.
        /// </summary>
        /// <param name="context">
        /// Struct that specifies the damage amount and contains other metadata about this damage instance.
        /// </param>
        /// <param name="attacker">
        /// A reference to the GameObject that is inflicting damage.
        /// </param>
        /// <returns>
        /// The actual damage dealt by this damage instance
        /// </returns>
        float Damage(DamageContext context, GameObject attacker);

        /// <summary>
        /// Increases this entity's health up to its maximum health value.
        /// </summary>
        /// <param name="context">
        /// Struct that specifies the healing amount and contains other metadata about this healing instance.
        /// </param>
        /// <param name="healer">
        /// A reference to the GameObject that is providing healing.
        /// </param>
        /// <returns>
        /// The actual healing provided by this healing instance
        /// </returns>
        float Heal(HealingContext context, GameObject healer);

    }

