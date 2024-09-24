using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    //initializing variables that can be dragged into in unity
    //speed of platform
    public float speed;
    //starting index position of platform
    public int startingPoint;
    //an array of transform points or where the platform needs to move
    public Transform[] points;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        //basically sets the position of the platform to position of points using starting point
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the distance between the points are too small then move to the next point
        if(Vector2.Distance(transform.position, points[index].position) < 0.02f) {
            //increases index
            index++;
            //chekcs if the platform is on the last point before resetting the index
            if (index == points.Length) {
                index = 0;
            }
        }
        //use vector2.moveTowards to move the platform
        //following format (position of platform, position of which it needs to move towards, speed)
        //delta time = smooth movement no matter how many fps
        transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
    }
}
