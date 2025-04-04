using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlagueDoctorSpecial : MonoBehaviour
{
    [HideInInspector] public SilasManager manager;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void StartDash()
    {
        GameObject bottle = Instantiate(manager.blightPotion, (Vector2)transform.position + manager.bottleOffset, transform.rotation);
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = PlayerID.instance.transform.position;
        bottle.GetComponent<Rigidbody2D>().velocity = (mousePos - playerPos).normalized * manager.initialSpeed;
        PlayerStateMachine psm = GetComponent<PlayerStateMachine>();
        psm.EnableTrigger("OPT");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + manager.bottleOffset, 0.3f);
    }
}
