using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code for moving platform moving at constant speed sequentially across multiple points
/// </summary>
public class movingPlatform : MonoBehaviour
{
    //initializing variables that can be dragged into in unity
    //speed of platform
    [SerializeField] float speed;

    //an array of vectors of where the platform needs to move
    private Vector2[] points;
    private int index;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // get list of points via children game object positions
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

    // Update is called once per frame
    void Update()
    {
        //checks if the distance between the points are too small then move to the next point
        if (Vector2.Distance(this.transform.position, points[index]) < 0.02f)
        {
            //increases index
            index++;
            //chekcs if the platform is on the last point before resetting the index
            if (index >= points.Length)
            {
                index = 0;
            }
        }
        //use vector2.moveTowards to move the platform
        //following format (position of platform, position of which it needs to move towards, speed)
        //delta time = smooth movement no matter how many fps
        Vector3 pos = this.transform.position;
        Vector2 vel = (points[index] - new Vector2(pos.x, pos.y)).normalized * speed;
        rb.velocity = vel;
    }
}
