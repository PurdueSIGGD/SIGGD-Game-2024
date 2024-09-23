using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour

{

    [SerializeField]
    float speed;

    // Start is called before the first frame update

    private InputAction moveAction;
    private Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 movement = moveAction.ReadValue<Vector2>();
        //movement = movement.normalized * speed;

        //float velY = rb.velocity.y;

        //rb.velocity.Set(movement.x, velY);

        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement = movement.normalized * speed;

        float yVel = rb.velocity.y;

        Vector3 newVelocity = new Vector3(movement.x, yVel, movement.y);
        rb.velocity = newVelocity;

    }
    private void FixedUpdate()
    {
           
    }
}
