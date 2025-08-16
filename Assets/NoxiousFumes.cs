using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoxiousFumes : Skill
{
    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 1f, 2f, 3f, 4f
    };
    private int pointIndex;

    [SerializeField] public GameObject poisonCloud;

    private SilasManager manager;



    // Start is called before the first frame update
    void Start()
    {
         manager = GetComponent<SilasManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void SpawnPoisonCloud(Vector3 position, float radius, float blightDuration)
    {
        if (pointIndex <= 0) return;

        GameObject cloudEntity = Instantiate(poisonCloud, position, Quaternion.identity);
        cloudEntity.GetComponent<PoisonCloud>().InitializePoisonCloud(manager, radius, values[pointIndex], blightDuration);
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
