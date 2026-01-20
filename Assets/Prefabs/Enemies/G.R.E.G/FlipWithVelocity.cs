using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWithVelocity : MonoBehaviour
{
    Rigidbody2D rb;
    float leftx;
    float rightx;
    float y;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftx = -gameObject.transform.localScale.x;
        rightx = gameObject.transform.localScale.x;
        y = gameObject.transform.localScale.y;
    }

    // Start is called before the first frame update
    void Update()
    {
        Vector3 vel = rb.velocity;    
        if (vel.x < 0)
        {
            gameObject.transform.localScale = new Vector3(leftx, y, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(rightx, y, 1);
        }
    }
}
