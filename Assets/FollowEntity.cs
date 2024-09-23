using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{ 
    [SerializeField]
    GameObject target; // Which entity is being followed

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    float maxForce;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        // Calculate the desired velocity of this object
        Vector2 desiredVelocity = target.transform.position - gameObject.transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the steering force
        Vector2 steer = desiredVelocity - rb.velocity;
        if (steer.magnitude > maxForce)
        {
            steer = maxForce * steer.normalized;
        }

        rb.AddForce(steer, ForceMode2D.Impulse);

    }

}
