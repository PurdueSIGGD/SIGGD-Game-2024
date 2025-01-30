using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class FastFall : MonoBehaviour
{
    private InputAction moveInput;
    private Rigidbody2D rb;

    [SerializeField] private float forceStrength = -100.0f;

    // Start is called before the first frame update
    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Fall");
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float input = moveInput.ReadValue<float>();
        Vector2 downwardForce = new Vector2(0.0f, forceStrength);
        if (input != 0f)
        {
            rb.AddForce(downwardForce);
        }
    }
}
