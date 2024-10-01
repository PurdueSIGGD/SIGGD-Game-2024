using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject indicator;

    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float swordDist;
    [SerializeField] GameObject grapple;

    private InputAction playerAction;
    private Rigidbody2D rb;
    private Boolean doubleJump = true;
    private Boolean onGround = false;
    private int counter = 0;
    private Camera mainCamera;

    
    private void Start()
    {
        playerAction = GetComponent<PlayerInput>().actions.FindAction("Movement");
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        Movement();

        if (indicator.activeSelf)
        {
            if (counter <= 0) {
                indicator.SetActive(false);
            }
            counter--;
        }

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 subV = gameObject.transform.position - mousePos;
        float angle = Mathf.Atan2(subV.y, subV.x);
        Vector2 offset = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle));
        indicator.transform.position = gameObject.transform.position + new Vector3(offset.x, offset.y, 0) * swordDist;
        indicator.transform.Rotate(0, 0, angle * 180/Mathf.PI - indicator.transform.eulerAngles.z);
    }

    private void Movement()
    {
        float input = playerAction.ReadValue<float>();
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
    }

    private void OnJump()
    {
        Debug.Log(onGround + " " + doubleJump);
        if (onGround || doubleJump){
            rb.AddForce(new Vector2(0, 1) * jumpPower, ForceMode2D.Impulse);
            if (!onGround){
                doubleJump = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        doubleJump = true;
        onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
    }

    private void OnHit()
    {
        indicator.SetActive(true);
        counter = 10;
    }

    private void OnGrapple()
    {

    }
}
