using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOrb : MonoBehaviour
{
    GameObject enemyPrefab;
    [SerializeField] Color orbColor;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] float spawnTime;
    float timer = 0;
    bool initialized = false;
    Action<GameObject> registerEnemy;
    Func<GameObject, bool> deregisterOrb;

    public void Initialize(GameObject enemyPrefab, Action<GameObject> registerEnemy, Func<GameObject, bool> deregisterOrb)
    {
        this.enemyPrefab = enemyPrefab;
        this.registerEnemy = registerEnemy;
        this.deregisterOrb = deregisterOrb;

        // visual
        // renderer.color = orbColor;
        // var main = particleSystem.main;
        // main.startColor = orbColor;

        initialized = true;
    }
    void Update()
    {
        if (initialized)
        {
            timer += Time.deltaTime;
            if (timer > spawnTime)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                registerEnemy(enemy);
                deregisterOrb(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
