using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage.
/// </summary>
public class Mage : EnemyStateManager
{
    private StatManager statManager;
    private MagesPlayerDetector playerDetector;
    private bool isPlayerInRange = false;

    private GameObject lightningObject;   // reference to the MageLightningAttack GameObject itself

    [Header("Lightning Attack")]

    //[SerializeField] private float lightningDamage = 25;
    [SerializeField] private DamageContext lightningDamage;
    [SerializeField] private float lightningRadius = 10;   // the size of the ACTUAL lightning attack
    [SerializeField] private float lightningRange = 10;   // denotes from how far away the mage can attack
    
    [SerializeField] private float lightningChargeDuration = 1f;  // how long in seconds the lightning is charging before damage is applied
    [SerializeField] private float cooldown = 2f;    // how long the mage is on cooldown after the lightning is discharged and applied damage (not when the attack starts charging)

    [SerializeField] private GameObject lightningPrefab;

    // cooldown private variables
    private float attackActivationTimestamp = 0f;   // this is the Time.time timestamp of when the attack starts casting
    private bool isFirstAttack = true;   // true means the mage doesn't need to wait for the lightningChargeDuration too



    protected void Start()
    {
        statManager = GetComponent<StatManager>();
        lightningDamage.damage = statManager.ComputeValue("Damage");
        // Subscribing to the detector's events
        playerDetector = GetComponentInChildren<MagesPlayerDetector>();
        if (playerDetector != null)
        {
            playerDetector.OnPlayerEnteredRange += OnPlayerEnteredRange;
            playerDetector.OnPlayerExitedRange += OnPlayerExitedRange;

            playerDetector.SetRange(lightningRange);
        }
    }


    private void Update()
    {
        if (isPlayerInRange && player != null)   // maybe check for aggro state, too?
        {
            // check cooldown
            float elapsedTime = Time.time - attackActivationTimestamp;   // time elapsed since the last attack activation
            float actual_cooldown = cooldown;
            
            if (isFirstAttack == false)
            {
                actual_cooldown += lightningChargeDuration;
            }

            if (elapsedTime > actual_cooldown)
            {
                Attack();
                //Debug.Log("MAGE ATTACK");
            }
        }
    }

    // Starting to cast lightning
    private void Attack()
    {
        attackActivationTimestamp = Time.time;

        // spawn a lightning prefab
        lightningObject = Instantiate(lightningPrefab, player.position, Quaternion.identity);

        MageLightningAttack attack = lightningObject.GetComponent<MageLightningAttack>();
        
        // creating damage context
        //DamageContext damageContext = new DamageContext();
        //damageContext.damage = lightningDamage;
        //damageContext.damageTypes = new List<DamageType>((int)DamageType.AREA);
        //damageContext.actionTypes = new List<ActionType>((int)ActionType.ENEMY_ATTACK);
        
        attack.Initialize(player.position, lightningRadius, lightningDamage, lightningChargeDuration, gameObject);
        attack.StartCharging();
    }

    private void OnPlayerEnteredRange(GameObject playerGameObject)
    {
        //Debug.Log("player entered mage range");

        // aggro
        //SwitchState(AggroState);

        isPlayerInRange = true;
        isFirstAttack = true;   // this means the mage doesn't need to wait for the lightningChargeDuration too

        // This is so that the Mage doesn't immediately cast an attack after player enters the range:
        attackActivationTimestamp = Time.time;
    }

    private void OnPlayerExitedRange(GameObject playerGameObject)
    {
        //Debug.Log("player exited mage range");

        // de-aggro
        //SwitchState(IdleState);

        isPlayerInRange = false;
    }


    private void OnDestroy()
    {
        // Unsubscribing from the detector's events
        if (playerDetector != null)
        {
            playerDetector.OnPlayerEnteredRange -= OnPlayerEnteredRange;
            playerDetector.OnPlayerExitedRange -= OnPlayerExitedRange;
        }
    }


    // Draws the Mage's attack range
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, lightningRange);
    }
}
