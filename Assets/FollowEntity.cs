using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    float maxSpeed; // The maximum speed of the desired velocity of this GameObject

    [SerializeField]
    float maxForce; // The maximum size of the steering force towards the target

    Rigidbody2D rb;

    /// <summary>
    /// Calculates and adds the steering force that will push this gameObject towards the target's position.
    /// </summary>
    private void Steer()
    {
        if (target == null) return;

        // Calculate the desired velocity of this object
        Vector2 desiredVelocity = target.transform.position - gameObject.transform.position;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the steering force
        Vector2 steerForce = desiredVelocity - rb.velocity;
        if (steerForce.magnitude > maxForce)
        {
            steerForce = maxForce * steerForce.normalized;
        }

        rb.AddForce(steerForce, ForceMode2D.Impulse);
    }

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
        Steer();
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
