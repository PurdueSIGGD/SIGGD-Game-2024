using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class FastFall : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float forceStrength = -100.0f;

    private bool isFastFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 downwardForce = new Vector2(0.0f, forceStrength);
        if (isFastFalling)
        {
            rb.AddForce(downwardForce);
        }
    }

    void StartFastFall()
    {
        isFastFalling = true;
    }

    void StopFastFall()
    {
        isFastFalling = false;
    }

}
