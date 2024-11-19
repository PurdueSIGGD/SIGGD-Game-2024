using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// A GameObject with the FollowEntity script follows a GameObject target (serialized field). 
/// Other serialized fields include offset for position, radius, and time multipliers
/// 
/// This version of FollowEntity does not overshoot and is only based on vector rotations.
/// 
/// </summary>
public class OscillateFollowEntity : MonoBehaviour, IParty
{ 
    [SerializeField]
    GameObject target; // Which entity is being followed

    [SerializeField]
    Vector2 offset; // Offset to target position

    [SerializeField]
    float radius; // the radius around center position that ghost revolves around

    [SerializeField]
    Vector3 timeMultipliers; // multiply time by these numbers when rotating by axis

    [SerializeField]
    Boolean showRadiusAndCenter; // for debugging

    Rigidbody2D rb;

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
