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
        

    }
    private void FixedUpdate()
    {

        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement = movement.normalized * speed;

        float yVel = rb.velocity.y;

        rb.velocity = new Vector2(movement.x, yVel);


    }
}
