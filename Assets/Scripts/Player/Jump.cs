using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Jump : MonoBehaviour, IStatList
{

    [SerializeField]
    public StatManager.Stat[] statList;

    private Rigidbody2D rb;
    private StatManager stats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();

    }

    public void StartJump()
    {
        rb.gravityScale = 4;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 1) * stats.ComputeValue("Jump Force"), ForceMode2D.Impulse);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
