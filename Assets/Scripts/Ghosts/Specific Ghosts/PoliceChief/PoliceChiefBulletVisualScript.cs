using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefBulletVisualScript : MonoBehaviour
{

    Transform initialPos;
    [SerializeField] float time;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform;
    }

    void Fire(Vector2 dir, float time, float speed, float range)
    {
        // rotate bullet to face player
        float angle = Vector2.Angle(Vector2.right * -1 * dir.x, dir.normalized);
        angle -= 180;
        int rot_flip = dir.x > 0 ? 0 : 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, rot_flip, angle));

        // set velocity and GOOO!!!
    }
    void Impact()
    {
        Destroy(gameObject);
    }
    // special animation for if bullet reaches max range but doesn't hit anything
    void Fizzle()
    {
        Destroy(gameObject);
    }
}
