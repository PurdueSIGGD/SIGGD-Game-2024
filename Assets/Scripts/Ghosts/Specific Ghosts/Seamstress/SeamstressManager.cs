using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeamstressManager : GhostManager
{
    [Header("Projectile")]
    [SerializeField] public static GameObject projectile;
    [SerializeField] public static float maxRicochet;
    [SerializeField] public static float flightSpeed;
    [SerializeField] public static float chainRange;
    [SerializeField] private static float chainedDuration;

    private static float durationCounter;
    private static float ricochetCounter;

    [Header("Fatebound Effect")]
    [SerializeField] private static float sharedDmgScaling;

    public static Queue<GameObject> linkableEnemies;
    private static ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private static ChainedEnemy tail; // should always point to the end of the list
    private static ChainedEnemy ptr;


    // Used to keep track of the all chaind enmeies
    class ChainedEnemy
    {
        public GameObject enemy;
        public ChainedEnemy chainedTo;

        public ChainedEnemy() { enemy = null; chainedTo = null; }
    }

    protected override void Start()
    {
        base.Start();
        ptr = head = tail = new ChainedEnemy();
        linkableEnemies = new Queue<GameObject>();
    }

    protected override void Update()
    {
        base.Update();
        if (head != null) // if linked list isn't empty
        {
            durationCounter -= Time.deltaTime;
            if (durationCounter < 0)
            {
                ClearList();
            }
        }
    }

    public override void Select(GameObject player)
    {
        base.Select(player);
        PlayerID.instance.AddComponent<YumeSpecial>();
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<YumeSpecial>()) Destroy(PlayerID.instance.GetComponent<YumeSpecial>());
    }

    public static void ResetDuration()
    {
        durationCounter = chainedDuration;
    }

    public static void AddEnemy(GameObject hitTarget)
    {
        if (head.enemy == null)
        {
            head.enemy = hitTarget;
            tail = head.chainedTo = new ChainedEnemy();
        }
        else
        {
            tail.enemy = hitTarget;
            tail.chainedTo = new ChainedEnemy();
            tail = tail.chainedTo;
        }
    }

    // should find the next closest enemy to the one given
    public static Transform FindNextTarget(GameObject cur)
    {
        Transform targetLoc = null;
        float minDist = chainRange;
        for (int i = 0; i < linkableEnemies.Count; i++)
        {
            GameObject enemy = linkableEnemies.Dequeue();

            if (enemy.GetInstanceID() == cur.GetInstanceID()) // if checking the currently linked enemy, pass
            {
                i--;
                continue; // do not add the removed enemy back to the list, the enemy is already linked
            }

            float dist = Vector2.Distance(enemy.transform.position, cur.transform.position);

            if (dist < minDist)
            {
                targetLoc = enemy.transform;
                minDist = dist;
            }

            linkableEnemies.Enqueue(enemy);
        }
        return targetLoc;
    }

    /// <summary>
    /// Call to share a damage taken by one enemy to all enemies with the fatebound
    /// effect.
    /// </summary>
    /// <param name="enemyID"> The instance id of the enemy currently being damaged </param>
    public static void DamageLinkedEnemies(int enemyID, DamageContext context)
    {
        ptr = head;

        while (ptr.enemy != null)
        {
            if (ptr.enemy.GetInstanceID() != enemyID)
            {
                DamageContext sharedDmg = new DamageContext();
                sharedDmg.damage = context.damage * sharedDmgScaling;
                sharedDmg.damageStrength = context.damageStrength;

                ptr.enemy.GetComponent<Health>().NoContextDamage(sharedDmg, PlayerID.instance.gameObject);
            }
            ptr = ptr.chainedTo;
        }
    }

    /// <summary>
    /// Remove an enemy from the linked list, used for when a chained enemy dies
    /// </summary>
    /// <param name="enemyID"> The instance id of the enemy being removed </param>
    public static void RemoveFromLink(int enemyID)
    {
        ptr = head;

        if (ptr.enemy.GetInstanceID() == enemyID)
        {
            head = head.chainedTo;
            return;
        }

        while(ptr.enemy != null)
        {
            if (ptr.chainedTo.enemy.GetInstanceID() == enemyID)
            {
                ptr.chainedTo = ptr.chainedTo.chainedTo;
                return;
            }
        }
    }

    /// <summary>
    /// Increment the number of times the current ability has ricocheted
    /// </summary>
    /// <returns> if the ability has reached its max ricochet amount </returns>
    public static bool IncrementRicochet()
    {
        ricochetCounter++;
        return ricochetCounter == maxRicochet;
    }

    private static void ClearList()
    {
        while (ptr.enemy != null)
        {
            Destroy(ptr.enemy.GetComponent<FateboundDebuff>());
            linkableEnemies.Enqueue(ptr.enemy);
            ptr = ptr.chainedTo;
        }
        ptr = head = tail = new ChainedEnemy();
    }
}
