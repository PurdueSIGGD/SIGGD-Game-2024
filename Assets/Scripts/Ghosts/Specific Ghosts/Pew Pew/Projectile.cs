using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the Projectile prefab
/// </summary>
public class Projectile : MonoBehaviour
{
    public float speed = 20;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
