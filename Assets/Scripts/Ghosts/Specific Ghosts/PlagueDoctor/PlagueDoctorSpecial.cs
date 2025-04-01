using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlagueDoctorSpecial : MonoBehaviour
{

    [SerializeField] GameObject blightPotion;
    [SerializeField] float initialSpeed;

    private Camera mainCamera;
    public PlagueDoctorManager manager;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDash()
    {
        GameObject bottle = Instantiate(blightPotion);
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = PlayerID.instance.transform.position;
        bottle.transform.position = playerPos;
        bottle.GetComponent<Rigidbody2D>().velocity = (mousePos - playerPos).normalized * initialSpeed;
        PlayerStateMachine psm = this.GetComponent<PlayerStateMachine>();
        psm.EnableTrigger("OPT");
    }

    void StopDash()
    {
        manager.setSpecialCooldown(4);
    }
}
