using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public GameObject pointA;
    
    public GameObject pointB;
    public float speed;
    private float curSpeed;

    private Rigidbody rb;
    private Transform currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = currentPoint.position - transform.position;

        if (currentPoint.position.x - transform.position.x > 0) {
                curSpeed = speed;
        }
        else {
                curSpeed = -speed;
        }

        if (currentPoint == pointB.transform){
            rb.velocity = new Vector3(curSpeed, 0, 0);
        }
        else {
            rb.velocity = new Vector3(curSpeed, 0, 0);
        }

        if (Vector3.Distance(transform.position, currentPoint.position) < 5f && currentPoint == pointB.transform) {
            print("change");
            currentPoint = pointA.transform;
        }
        if (Vector3.Distance(transform.position, currentPoint.position) < 5f && currentPoint == pointA.transform) {
            print("change");
            currentPoint = pointB.transform;
        }
    }
}
