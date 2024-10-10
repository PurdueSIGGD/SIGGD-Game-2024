using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    private InputAction movement;
    [SerializeField]
    float speed = 3.0f;

    [SerializeField]
    Stat Attack = new Stat("Attack", 100, 0);
    [SerializeField]
    Stat Defense = new Stat("Defense", 100, 0);

    [SerializeField]
    Stat Range = new Stat("Range", 100, 0);

    public Stats GetAttack() {
        return Attack;
    }
    public Stats GetDefense() {
        return Range;
    }
    public Stats GetRange() {
        return Range;
    }

    //public bool[] inParty;

    void Start()
    {
        movement = this.GetComponent<PlayerInput>().actions.FindAction("Movement");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = movement.ReadValue<Vector2>();
        Vector3 newPos = this.transform.position + (new Vector3(input.x, input.y, 0)) * Time.deltaTime * speed;
        this.transform.position = newPos;
    }
}
