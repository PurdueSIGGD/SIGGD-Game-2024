using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class ElectricStep : Skill
{
    [SerializeField]
    List<int> values = new List<int>
    {
        0, 10, 20, 30, 40
    };
    [HideInInspector] public IdolManager manager;
    [SerializeField] GameObject fieldVisual;
    GameObject fieldVisualInstance;

    [SerializeField] DamageContext damage;
    int dps;
    float damageTick; // the amount of damage
    [SerializeField] float radius;
    bool fieldActive;
    private static int pointIndex;

    void Start()
    {
        manager = gameObject.GetComponent<IdolManager>();
        manager.evaSelectedEvent.AddListener(EvaSelected);

        fieldVisualInstance = Instantiate(fieldVisual, PlayerID.instance.gameObject.transform);
        fieldVisualInstance.transform.localScale = new Vector3(1, 1, 1) * radius;
        fieldVisualInstance.SetActive(false);
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }
    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }
    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }
    void Update()
    {

        // toggle electric field on and off depending on if eva is active and has max stacks, and has positive number of skillpoints

        bool toggle = manager.passive.active &&
                    manager.passive.tempoStacks == (int)manager.GetStats().ComputeValue("TEMPO_MAX_STACKS") &&
                    pointIndex > 0;
        ToggleField(toggle);

        if (fieldActive)
        {

            // search all enemies hit by electricity in the radius

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            int count = 0;
            foreach (Collider2D hit in hitColliders)
            {
                count++;

                // apply damage tick every frame to every enemy in the circle radius

                print("EFIELD: " + hit.gameObject.name);
                Health enemyHealth = hit.GetComponent<Health>();
                dps = values[pointIndex];
                damageTick = dps * Time.deltaTime;
                damage.damage = damageTick;
                enemyHealth.Damage(damage, PlayerID.instance.gameObject);
                //print("EFIELD HP: " + 1 + " " + enemyHealth.currentHealth);
            }
            print("EFIELD COUNT: " + count);
        }
    }
    private void UpdateSkill()
    {
        dps = values[pointIndex];
        damageTick = dps * Time.deltaTime;
        damage.damage = damageTick;
    }
    public void ToggleField(bool fieldActive)
    {
        this.fieldActive = fieldActive;

        // set visual active accordingly

        if (!fieldVisualInstance.activeSelf && fieldActive)
        {
            fieldVisualInstance.SetActive(true);
        }
        else if (fieldVisualInstance.activeSelf && !fieldActive)
        {
            fieldVisualInstance.SetActive(false);
        }
    }
    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    private void EvaSelected()
    {
        if (pointIndex == 0)
        {
            return;
        }
        manager.passive.avaliableHoloJumpVA.Add("Eva-Idol Electric Step");
    }
}