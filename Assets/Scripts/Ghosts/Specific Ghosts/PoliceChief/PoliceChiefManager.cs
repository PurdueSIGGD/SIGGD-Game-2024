using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefManager : MonoBehaviour, ISelectable
{
    private Animator animator;

    [SerializeField]
    public RuntimeAnimatorController defaultController;
    public RuntimeAnimatorController policeChiefController;
    void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
    }
    // ISelectable interface in use
    public void Select(GameObject player)
    {
        PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        Destroy(PlayerID.instance.GetComponent<Dash>());
        // change animation controller
        animator.runtimeAnimatorController = policeChiefController;
    }

    public void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<PoliceChiefSpecial>() == true)
        {
            Destroy(PlayerID.instance.GetComponent<PoliceChiefSpecial>());
        }
        if (PlayerID.instance.GetComponent<Dash>() == false)
        {
            PlayerID.instance.AddComponent<Dash>();
        }
        //change back to default player controller
        animator.runtimeAnimatorController = defaultController;
    }
}
