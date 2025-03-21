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
    public StatManager stats;
    void Start()
    {
        animator = PlayerID.instance.GetComponent<Animator>();
    }
    public GameObject basicProjectile; // visual purposes only
    // ISelectable interface in use
    public void Select(GameObject player)
    {
        PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        Destroy(PlayerID.instance.GetComponent<Dash>());

        PlayerID.instance.AddComponent<PoliceChiefBasic>().SetStats(stats);
        Destroy(PlayerID.instance.GetComponent<LightAttack>());

        // change animation controller
        animator.runtimeAnimatorController = policeChiefController;
    }

    public void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<PoliceChiefSpecial>() == true)
        {
            Destroy(PlayerID.instance.GetComponent<PoliceChiefSpecial>());
            Destroy(PlayerID.instance.GetComponent<PoliceChiefBasic>());
        }
        if (PlayerID.instance.GetComponent<Dash>() == false)
        {
            PlayerID.instance.AddComponent<Dash>();
            PlayerID.instance.AddComponent<LightAttack>();
        }
        //change back to default player controller
        animator.runtimeAnimatorController = defaultController;
    }
}
