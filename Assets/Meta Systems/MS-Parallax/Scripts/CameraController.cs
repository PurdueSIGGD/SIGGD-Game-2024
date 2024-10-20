using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputAction moveInput;
    [SerializeField] private float speed;

    void OnEnable() 
    {
        moveInput.Enable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 moveDirection = moveInput.ReadValue<Vector2>();
        Vector3 deltaPosition = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;
        transform.position += deltaPosition * speed * Time.deltaTime;
    }
}
