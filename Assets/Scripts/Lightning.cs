using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For the lightning prefab
public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 25;
    public float lifeTime = 0.4f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
