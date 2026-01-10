using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject dummyEnemy;
    [SerializeField] private GameObject reviveTarget;

    private GameObject currentAlive;

    void OnEnable()
    {
        GameplayEventHolder.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath += OnDeath;
    }

    void Start()
    {
        Spawn();
    }

    private void OnDeath(DamageContext damage)
    {
        if (damage.victim == currentAlive)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        currentAlive = Instantiate(dummyEnemy);
        currentAlive.transform.position = reviveTarget.transform.position;
    }
}
