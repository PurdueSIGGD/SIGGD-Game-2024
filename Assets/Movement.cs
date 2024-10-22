using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    private InputAction movement;

    //public bool[] inParty;
    private int moveStatIdx;
    private Stats stats;

    void Start()
    {
        movement = this.GetComponent<PlayerInput>().actions.FindAction("Movement");
        stats = this.GetComponent<Stats>();
        moveStatIdx = stats.GetStatIndex("Movement");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = movement.ReadValue<Vector2>();
        float speed = stats.ComputeValue(moveStatIdx);
        Vector3 newPos = this.transform.position + (new Vector3(input.x, input.y, 0)) * Time.deltaTime * speed;
        this.transform.position = newPos;
    }
}
