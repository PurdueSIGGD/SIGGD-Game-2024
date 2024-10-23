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
    //starting index position of platform
    public int startingPoint;
    [SerializeField] GameObject platform;
    //an array of vectors of where the platform needs to move
    [SerializeField] Transform[] points;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        //basically sets the position of the platform to position of points using starting point
        platform.transform.position = points[startingPoint].position;
        index = startingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the distance between the points are too small then move to the next point
        if (Vector2.Distance(platform.transform.position, points[index].position) < 0.02f)
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
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, points[index].position, speed * Time.deltaTime);
    }
}
