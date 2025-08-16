using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisController : MonoBehaviour
{
    [SerializeField] GameObject bossControllerPrefab;
    BossController bossController;
    [SerializeField] bool startSpawn;

    // Start is called before the first frame update
    void Start()
    {
        GameObject bossControllerInstance = Instantiate(bossControllerPrefab, transform.position, Quaternion.identity);
        bossController = bossControllerInstance.GetComponent<BossController>();
        bossController.Initialize(this.gameObject);
    }
    void Update()
    {
        if (startSpawn)
        {
            bossController.StartSpawning();
            startSpawn = false;
        }
    }
}
