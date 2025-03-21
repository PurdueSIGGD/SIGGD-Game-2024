using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class IdolManager : MonoBehaviour, ISelectable
{
    private Animator animator;

    [SerializeField]
    public RuntimeAnimatorController defaultController;
    //public RuntimeAnimatorController policeChiefController;
    [SerializeField] public GameObject idolClone;
    void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
    }
    // ISelectable interface in use
    public void Select(GameObject player)
    {
        Debug.Log("EVA SELECTED!");
        PlayerID.instance.AddComponent<IdolPassive>();
        PlayerID.instance.AddComponent<IdolSpecial>();
        PlayerID.instance.GetComponent<IdolSpecial>().idolClone = idolClone;
        Destroy(PlayerID.instance.GetComponent<Dash>());
        // change animation controller
        animator.runtimeAnimatorController = defaultController;
    }

    public void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<IdolPassive>() == true)
        {
            Destroy(PlayerID.instance.GetComponent<IdolPassive>());
        }
        if (PlayerID.instance.GetComponent<IdolSpecial>() == true)
        {
            Destroy(PlayerID.instance.GetComponent<IdolSpecial>());
        }
        if (PlayerID.instance.GetComponent<Dash>() == false)
        {
            PlayerID.instance.AddComponent<Dash>();
        }
        //change back to default player controller
        animator.runtimeAnimatorController = defaultController;
    }
}