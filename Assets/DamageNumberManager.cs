using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    [HideInInspector] public static DamageNumberManager instance;

    [SerializeField] private GameObject damageNumber;
    [SerializeField] private GameObject healingNumber;

    private List<DamageNumber> activeDamageNumbers;
    private List<DamageNumber> activeHealingNumbers;





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

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeDamageNumbers = new List<DamageNumber>();
        activeHealingNumbers = new List<DamageNumber>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public void AddToActiveDamageNumbers(DamageNumber damageNumber)
    {
        activeDamageNumbers.Add(damageNumber);
    }

    public void RemoveFromActiveDamageNumbers(DamageNumber damageNumber)
    {
        activeDamageNumbers.Remove(damageNumber);
    }

    public void AddToActiveHealingNumbers(DamageNumber healingNumber)
    {
        activeHealingNumbers.Add(healingNumber);
    }

    public void RemoveFromActiveHealingNumbers(DamageNumber healingNumber)
    {
        activeHealingNumbers.Remove(healingNumber);
    }





    public void DamageNumberEvent(DamageContext context)
    {
        if (GetActiveDamageNumber(context.victim) != null) return;
        GameObject newDamageNumber = Instantiate(damageNumber, new Vector3(-999f, -999f, 0f), Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().InitializeMessage(this, context.victim, context);
    }

    public void HealingNumberEvent(HealingContext context)
    {
        if (GetActiveHealingNumber(context.healee) != null) return;
        GameObject newDamageNumber = Instantiate(healingNumber, new Vector3(-999f, -999f, 0f), Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().InitializeMessage(this, context.healee, context);
    }

    public void PlayMessage(GameObject owner, float value, Sprite icon, string message, Color color)
    {
        DamageNumber activeDamageNumber = GetActiveDamageNumber(owner);
        if (activeDamageNumber != null)
        {
            activeDamageNumber.PlayMessage(value, icon, message, color, true);
            return;
        }
        GameObject newDamageNumber = Instantiate(damageNumber, new Vector3(-999f, -999f, 0f), Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().InitializeMessage(this, owner, value, icon, message, color);
    }





    private DamageNumber GetActiveDamageNumber(GameObject owner)
    {
        foreach (DamageNumber damageNumber in activeDamageNumbers)
        {
            if (damageNumber.GetOwner().Equals(owner))
            {
                return damageNumber;
            }
        }
        return null;
    }

    private DamageNumber GetActiveHealingNumber(GameObject owner)
    {
        foreach (DamageNumber healingNumber in activeHealingNumbers)
        {
            if (healingNumber.GetOwner().Equals(owner))
            {
                return healingNumber;
            }
        }
        return null;
    }
}
