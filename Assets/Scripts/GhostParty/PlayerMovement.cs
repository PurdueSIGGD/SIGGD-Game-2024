using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Separate Movement from Party scripts in two different scripts
    public InputAction moveAction;
    public Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;
    [SerializeField]
    float speed;

    void OnEnable() {
        moveAction.Enable();
    }

    void OnDisable() {
        moveAction.Disable();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        moveDirection = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        rb.velocity = new Vector3(moveDirection.x * speed, moveDirection.y * speed, 0);
    }

}