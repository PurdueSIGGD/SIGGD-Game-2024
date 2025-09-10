using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Bloodletting : Skill
{
    [SerializeField]
    List<int> values = new List<int>
    {
        0, 1, 2, 3, 4
    };
    public int pointIndex;

    [SerializeField] private float lowChanceMultiplier;

    private SilasManager manager;



    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += DropOnDamage;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= DropOnDamage;
    }



    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<SilasManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void DropOnDamage(DamageContext context)
    {
        if (pointIndex <= 0) return;
        if (manager.healReady) return;
        if (context.damageTypes.Contains(DamageType.STATUS)) return;
        if (!context.victim.CompareTag("Enemy") || context.victim.GetComponentInChildren<BlightDebuff>() == null) return;

        // Determine drop chance
        float dropChance = values[pointIndex];
        if (context.victim.GetComponent<DropTable>() == null ||
            context.actionID == ActionID.PLAGUE_DOCTOR_SPECIAL)
        {
            dropChance *= lowChanceMultiplier;
        }

        // Drop ingredient
        if (Random.Range(0f, 100f) <= dropChance)
        {
            manager.DropIngredient(context.victim.transform.position);
        }
    }



    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
}
