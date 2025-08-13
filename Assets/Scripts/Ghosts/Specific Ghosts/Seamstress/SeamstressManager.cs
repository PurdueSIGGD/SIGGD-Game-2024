using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeamstressManager : GhostManager
{
    private YumeSpecial special;
    private YumeHeavy heavy;
    [SerializeField] ActionContext onSpoolGained;

    private int spools;
    private float spoolTimer;

    [Header("Projectile")]
    public GameObject projectile;
    public DamageContext projectileDamageContext;

    private float durationCounter;
    private float ricochetCounter;

    [Header("Fatebound Effect")]
    public ActionContext specialContext;
    [SerializeField] private DamageContext sharedDmg;
    [SerializeField] private float sharedDmgScaling;

    public Queue<GameObject> linkableEnemies;
    private ChainedEnemy head; // will usually be the first enemy hit by Yume's projectile
    private ChainedEnemy tail; // should always point to the end of the list
    private ChainedEnemy ptr;

    private LineRenderer lineRenderer;

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
        lineRenderer = GetComponent<LineRenderer>();

        durationCounter = GetStats().ComputeValue("Fatebound Duration");
        spools = SaveManager.data.yume.spoolCount;
    }

    protected override void Update()
    {
        base.Update();
        UpdateLinkedEnemies();
    }

    public override void Select(GameObject player)
    {
        base.Select(player);
        special = PlayerID.instance.AddComponent<YumeSpecial>();
        special.manager = this;
        heavy = PlayerID.instance.AddComponent<YumeHeavy>();
        heavy.manager = this;
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<YumeSpecial>()) Destroy(PlayerID.instance.GetComponent<YumeSpecial>());
        if (PlayerID.instance.GetComponent<YumeHeavy>()) Destroy(PlayerID.instance.GetComponent<YumeHeavy>());
        base.DeSelect(player);
    }

    public int GetSpools()
    {
        return spools;
    }

    public void AddSpools(int nspools)
    {
        spools = (int) Math.Clamp(spools + nspools, 0, stats.ComputeValue("Max Spools"));
        SaveManager.data.yume.spoolCount = spools;
        if (nspools > 0)
        {
            GameplayEventHolder.OnAbilityUsed.Invoke(onSpoolGained);
        }
    }

    public void SetWeaveTimer(float time)
    {
        spoolTimer = time;
    }

    public float GetWeaveTimer()
    {
        return spoolTimer;
    }

    public void ResetDuration()
    {
        durationCounter = GetStats().ComputeValue("Fatebound Duration");
    }

    public void AddEnemy(GameObject hitTarget)
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
        int count = ++lineRenderer.positionCount;
        lineRenderer.SetPosition(count - 1, hitTarget.transform.position);
    }

    // should find the next closest enemy to the one given
    public Transform FindNextTarget(GameObject cur)
    {
        Transform targetLoc = null;
        float minDist = GetStats().ComputeValue("Projectile Enemy Chain Range");
        for (int i = 0; i < linkableEnemies.Count; i++)
        {
            GameObject enemy = linkableEnemies.Dequeue();

            if (enemy == null || enemy.GetInstanceID() == cur.GetInstanceID()) // if checking the currently linked enemy, pass
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
    /// <param name="scaleDamageStrength"> Whether to scale the damage strength by sharedDmgScaling </param>
    public void DamageLinkedEnemies(int enemyID, DamageContext context, bool scaleDamageStrength)
    {
        ptr = head;

        while (ptr.enemy != null)
        {
            if (ptr.enemy.GetInstanceID() != enemyID)
            {
                // when damaging an enemy through fatebound effect, only damage, 
                // damagestrength, and the victim will be set according to the origional damage
                // action type and damage type will be preset in the editor
                sharedDmg.damage = context.damage;
                if (scaleDamageStrength)
                {
                    sharedDmg.damage *= sharedDmgScaling;
                }
                sharedDmg.damageStrength = context.damageStrength;
                sharedDmg.victim = ptr.enemy;

                //ptr.enemy.GetComponent<Health>().NoContextDamage(sharedDmg, PlayerID.instance.gameObject);
                ptr.enemy.GetComponent<Health>().Damage(sharedDmg, PlayerID.instance.gameObject);
            }
            ptr = ptr.chainedTo;
        }
    }

    /// <summary>
    /// Remove an enemy from the linked list, used for when a chained enemy dies
    /// </summary>
    /// <param name="enemyID"> The instance id of the enemy being removed </param>
    public void RemoveFromLink(int enemyID)
    {

        ptr = head;

        if (ptr.enemy != null && ptr.enemy.GetInstanceID() == enemyID)
        {
            head = head.chainedTo;
            return;
        }

        while (ptr.enemy != null)
        {
            if (ptr.chainedTo.enemy.GetInstanceID() == enemyID)
            {
                ptr.chainedTo = ptr.chainedTo.chainedTo;
                return;
            }
            ptr = ptr.chainedTo;
        }
    }

    /// <summary>
    /// Increment the number of times the current ability has ricocheted
    /// </summary>
    /// <returns> if the ability has reached its max ricochet amount </returns>
    public bool IncrementRicochet()
    {
        ricochetCounter++;
        return ricochetCounter == GetStats().ComputeValue("Projectile Ricochet Count");
    }

    public void ResetRicochet()
    {
        ricochetCounter = 0;
    }

    private void ClearList()
    {
        ptr = head;
        while (ptr.enemy != null)
        {
            FateboundDebuff debuff = ptr.enemy.GetComponent<FateboundDebuff>();
            debuff.RemoveShareDamage();
            Destroy(debuff);
            ptr = ptr.chainedTo;
        }
        ptr = head = tail = new ChainedEnemy();
        lineRenderer.positionCount = 0;
    }

    private void UpdateLinkedEnemies()
    {
        int i = 0; // used to keep track of each point used in line-render
        if (head.enemy != null) // if linked list isn't empty
        {
            durationCounter -= Time.deltaTime;
            if (durationCounter < 0)
            {
                ClearList();
            }

            // draw a line of connection between each enemy
            ptr = head;

            while (ptr.enemy != null)
            {
                lineRenderer.SetPosition(i, ptr.enemy.transform.position);
                i++;
                ptr = ptr.chainedTo;
            }
        }
        lineRenderer.positionCount = i; // clear any extra points left behind when an enemy dies
    }
}
