using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{

    [SerializeField] private GameObject damageNumber;
    [SerializeField] private GameObject healingNumber;

    private List<GameObject> activeDamageOwners;
    private List<GameObject> activeHealingOwners;





    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += DamageNumberEvent;
        GameplayEventHolder.OnHealingDealt += HealingNumberEvent;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= DamageNumberEvent;
        GameplayEventHolder.OnHealingDealt -= HealingNumberEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeDamageOwners = new List<GameObject>();
        activeHealingOwners = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public void AddToActiveDamageOwners(GameObject owner)
    {
        activeDamageOwners.Add(owner);
    }

    public void RemoveFromActiveDamageOwners(GameObject owner)
    {
        activeDamageOwners.Remove(owner);
    }

    public void AddToActiveHealingOwners(GameObject owner)
    {
        activeHealingOwners.Add(owner);
    }

    public void RemoveFromActiveHealingOwners(GameObject owner)
    {
        activeHealingOwners.Remove(owner);
    }





    public void DamageNumberEvent(DamageContext context)
    {
        if (activeDamageOwners.Contains(context.victim)) return;
        GameObject newDamageNumber = Instantiate(damageNumber, new Vector3(-999f, -999f, 0f), Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().InitializeMessage(this, context.victim, context);
    }

    public void HealingNumberEvent(HealingContext context)
    {
        if (activeHealingOwners.Contains(context.healee)) return;
        GameObject newDamageNumber = Instantiate(healingNumber, new Vector3(-999f, -999f, 0f), Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().InitializeMessage(this, context.healee, context);
    }
}
