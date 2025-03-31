using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Placeholder class for testing yume speical ability
/// Expect to be replaced after enemy spawn is implemented
/// </summary>
[DisallowMultipleComponent]
public class EnemySetTest : MonoBehaviour
{
    public static EnemySetTest Instance;
    public static Queue<GameObject> enemies = new Queue<GameObject>();
    void Start()
    {
        Instance = this;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Enqueue(gameObject);
        }
    }
}
