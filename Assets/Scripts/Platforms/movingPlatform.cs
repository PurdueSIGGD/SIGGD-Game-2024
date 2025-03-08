using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// Code for moving platform moving at constant speed sequentially across multiple points
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    //initializing variables that can be dragged into in unity
    //speed of platform
    [SerializeField] float speed;
    [SerializeField] bool loop;

    //an array of vectors of where the platform needs to move
    private Vector2[] points;
    private int index;

    private Rigidbody2D playerRB = null;
    private bool isTouchingPlayer = false;
    private int increment;

    // Start is called before the first frame update
    void Start()
    {
        increment = 1;

        // get list of points via children game object positions
        if (transform.childCount == 0)
        {
            GameObject empty = new GameObject();
            Instantiate(empty, this.transform.position, Quaternion.identity,  this.transform);
        }

        points = new Vector2[transform.childCount];
        int temp = 0;
        foreach (Transform child in transform)
        {
            points[temp] = child.position;
            temp++;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //basically sets the position of the platform to position of points using starting point
        index = 0;
        this.transform.position = points[index];
    }

    void FixedUpdate()
    {
        //checks if the distance between the points are too small then move to the next point
        if (Vector2.Distance(this.transform.position, points[index]) < 0.02f)
        {
            if (loop)
            {
                index++;
                if (index >= points.Length)
                {
                    index = 0;
                }
            } else
            {
                index += increment;
                if (index >= points.Length)
                {
                    index = points.Length - 1;
                    increment = -1;
                } else if (index < 0)
                {
                    index = 0;
                    increment = 1;
                }
            }
        }
        //use vector2.moveTowards to move the platform
        //following format (position of platform, position of which it needs to move towards, speed)
        //delta time = smooth movement no matter how many fps

        Vector3 newPos = Vector3.MoveTowards(transform.position, points[index], Time.deltaTime * speed);
        if (isTouchingPlayer)
        {
            PlayerID.instance.transform.position += newPos - transform.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, points[index], Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            isTouchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
}
