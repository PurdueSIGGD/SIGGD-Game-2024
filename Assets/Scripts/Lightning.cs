using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves Lightning prefab downward
/// </summary>
public class Lightning : MonoBehaviour
{
    public float speed = 25;
    public float lifeTime = 0.4f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
