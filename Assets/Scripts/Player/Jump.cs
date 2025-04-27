using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class Jump : MonoBehaviour, IStatList
{

    [SerializeField]
    public StatManager.Stat[] statList;

    private Rigidbody2D rb;
    private StatManager stats;
    public InputAction jumpAction;
    public PlayerStateMachine psm;
    bool jumping;

    void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
        jumpAction = GetComponent<PlayerInput>().actions.FindAction("Jump");
    }

    public void StartJump()
    {
        rb.gravityScale = 4;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 1) * stats.ComputeValue("Jump Force"), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (psm.currentAnimation == "player_jump")
        {
            float jumpValue = jumpAction.ReadValue<float>();
            if (jumpValue == 0 && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            }
        }
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
