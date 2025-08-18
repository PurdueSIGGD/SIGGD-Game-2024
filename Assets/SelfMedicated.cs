using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfMedicated : Skill
{
    [SerializeField]
    List<int> values = new List<int>
    {
        0, 1, 2, 3, 4
    };
    private int pointIndex;

    [SerializeField] public float blightBuffDuration;
    [SerializeField] public float apothecaryBuffDuration;

    [HideInInspector] public float buffDuration;
    [HideInInspector] public float timer = 0f;
    [HideInInspector] public bool isBuffed = false;

    [HideInInspector] private SilasManager manager;



    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<SilasManager>();
        PlayerID.instance.gameObject.GetComponent<PlayerHealth>().silasSelfMedicated = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f && isBuffed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                RemoveBuff();
            }
        }
    }



    public void ApplyBuff(float duration)
    {
        if (pointIndex <= 0 || !manager.isSelected) return;
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Max Running Speed", values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Running Accel.", values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Max Glide Speed", values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Glide Accel.", values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Dodge Chance", values[pointIndex] * 10);
        isBuffed = true;
        timer = duration;
        buffDuration = duration;
    }

    public void AddBuffTime(float time)
    {
        if (pointIndex <= 0 || !manager.isSelected) return;
        timer += time;
        buffDuration = Mathf.Max(buffDuration, timer);
    }

    public void SetBuffTime(float time)
    {
        if (pointIndex <= 0 || !manager.isSelected) return;
        timer = Mathf.Max(time, timer);
        buffDuration = Mathf.Min(buffDuration, timer);
    }

    public void RemoveBuff()
    {
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Max Running Speed", -values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Running Accel.", -values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Max Glide Speed", -values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Glide Accel.", -values[pointIndex]);
        PlayerID.instance.GetComponent<StatManager>().ModifyStat("Dodge Chance", -(values[pointIndex] * 10));
        isBuffed = false;
        timer = 0f;
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
