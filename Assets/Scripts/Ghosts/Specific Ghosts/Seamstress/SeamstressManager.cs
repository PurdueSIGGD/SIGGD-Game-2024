using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeamstressManager : MonoBehaviour, ISelectable
{

    private Animator animator;

    [SerializeField]
    public RuntimeAnimatorController defaultController;
    public RuntimeAnimatorController seamstressController;
    void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
    }

    public void Select(GameObject player)
    {
        // Add Seamstress Abilities 
        PlayerID.instance.AddComponent<SeamstressBasicSpiritLoom>();

        // Remove Default
        // Destroy(PlayerID.instance.GetComponent<FastFall>());

        // Change Animation Controller
        animator.runtimeAnimatorController = seamstressController;

    }

    public void DeSelect(GameObject player)
    {

        if (PlayerID.instance.GetComponent<SeamstressBasicSpiritLoom>())
        {
            Destroy(PlayerID.instance.GetComponent<SeamstressBasicSpiritLoom>());
        }
        //if (!PlayerID.instance.GetComponent<FastFall>())
        //{
        //    PlayerID.instance.AddComponent<FastFall>();
        //}

        // change back to default player controller
        animator.runtimeAnimatorController = defaultController;
    }

    //
}
