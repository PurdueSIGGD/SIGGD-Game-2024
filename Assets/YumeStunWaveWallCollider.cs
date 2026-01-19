using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeStunWaveWallCollider : MonoBehaviour
{
    private YumeStunWave stunWave;

    // Start is called before the first frame update
    void Start()
    {
        stunWave = GetComponentInParent<YumeStunWave>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!stunWave.isWaving) return;
        stunWave.EndWave();
    }
}
