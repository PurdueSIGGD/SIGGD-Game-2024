using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Allow the enemy to patroll indefinitely between point A
/// and B.
/// </summary>
public class EnemyPatroll : MonoBehaviour
{
    public GameObject pointA; // Patroll endpoint A
    public GameObject pointB; // Patroll endpoint B
    public float speed; // Enemy patroll speed property
    private float curSpeed; // Enemy's current speed

    private Rigidbody rb; // Enemy body
    private Transform currentPoint; // Enemy transform property

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }


    /// <summary>
    /// Update the enemy's location by moving with respect to speed property.
    /// </summary>
    private void move()
    {
        Vector3 point = currentPoint.position - transform.position;

        if (currentPoint.position.x - transform.position.x > 0)
        {
            curSpeed = speed;
        }
        else
        {
            curSpeed = -speed;
        }

        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector3(curSpeed, 0, 0);
        }
        else
        {
            rb.velocity = new Vector3(curSpeed, 0, 0);
        }

        if (Vector3.Distance(transform.position, currentPoint.position) < 5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        if (Vector3.Distance(transform.position, currentPoint.position) < 5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
    }
}
