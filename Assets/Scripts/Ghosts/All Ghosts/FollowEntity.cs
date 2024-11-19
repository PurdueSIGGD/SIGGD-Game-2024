using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A GameObject with the FollowEntity script follows a GameObject target (serialized field). 
/// Other serialized fields include maxSpeed and maxForce.
/// Source: https://natureofcode.com/autonomous-agents/
/// </summary>
public class FollowEntity : MonoBehaviour, IParty
{ 
    [SerializeField]
    GameObject target; // Which entity is being followed

    //[SerializeField]
    //float maxSpeed; // The maximum speed of the desired velocity of this GameObject

    //[SerializeField]
    //float maxForce; // The maximum size of the steering force towards the target

    [SerializeField]
    Vector2 offset; // Offset to target position

    [SerializeField]
    float radius; // the radius around center position that ghost revolves around
    // desired separation distance will be radius * 2

    [SerializeField]
    Vector3 timeMultipliers; // multiply time by these numbers when rotating by axis

    [SerializeField]
    Boolean showRadiusAndCenter; // for debugging

    Rigidbody2D rb;

    /// <summary>
    /// Calculates and adds the steering force that will push this gameObject towards the target's position.
    /// </summary>
    //private void Steer()
    //{
    //    if (target == null) return;

    //    // target plus offset
    //    Vector3 desiredPosition = target.transform.position + new Vector3(offset.x, offset.y, 0);

    //    // Calculate the desired velocity of this object
    //    Vector2 desiredVelocity = desiredPosition - gameObject.transform.position;
    //    desiredVelocity = desiredVelocity.normalized * maxSpeed;

    //    // Calculate the steering force
    //    Vector2 steerForce = desiredVelocity - rb.velocity;

    //    steerForce = maxForce * steerForce.normalized;

    //    rb.AddForce(steerForce, ForceMode2D.Impulse);

    //}

    /// <summary>
    /// Calculates and adds the steering force that will push this gameObject towards the target's position.
    /// </summary>
    private void Oscillate()
    {
        if (target == null) return;

        // entity will revolve around this position
        Vector3 centerPosition = target.transform.position;
        centerPosition.y += offset.y;
        centerPosition.x += offset.x;

        Vector3 movement = new Vector3(radius, radius, radius);

        movement = Quaternion.AngleAxis(Time.frameCount * timeMultipliers.x, Vector3.forward) * movement;
        movement = Quaternion.AngleAxis(Time.frameCount * timeMultipliers.y, Vector3.right) * movement;
        movement = Quaternion.AngleAxis(Time.frameCount * timeMultipliers.z, Vector3.up) * movement;

        rb.transform.position = centerPosition + movement;

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Steer();
        Oscillate();
    }

    public void EnterParty(GameObject player)
    {
        this.target = player;
    }

    public void ExitParty(GameObject player)
    {
        this.target = null;
        this.rb.velocity = new Vector2(0, 0);
    }

}
