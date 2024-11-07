using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Start is called before the first frame update
    public interface IDamageable
    {
        /// <summary>
        /// Method Shared with all classes with I_Damageable
        /// </summary>
        /// <param name="damage"></param>
        /// Takes a damage from player or some sort of attacker/enemy or Enviromental object
        /// <param name="arbitraryHits"></param>
        /// If true, then only takes 3 hits/calls to the method, ignores damage count
        void TakeDamage(float damage); 

    }

