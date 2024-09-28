using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For the projectile prefab
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 20;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
