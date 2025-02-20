using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D _rb;
    private float jumpforce = 10;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;

        //movement
        if (Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(Vector3.up * jumpforce);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveright();
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveleft();
        }

    }
    public void moveleft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    public void moveright()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
   
}