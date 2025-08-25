using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrologueHubUnlockDashGlide : MonoBehaviour
{
    [SerializeField] bool unlockGlide;
    [SerializeField] bool unlockDash;

    private Animator anim;
    void Start()
    {
        anim = PlayerID.instance.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (unlockDash)
            {
                anim.SetBool("Can_Dash", true);
            }
            if (unlockGlide)
            {
                anim.SetBool("Can_Glide", true);
            }
        }
    }
}
