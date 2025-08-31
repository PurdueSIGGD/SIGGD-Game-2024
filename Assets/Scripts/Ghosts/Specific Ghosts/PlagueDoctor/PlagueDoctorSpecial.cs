using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlagueDoctorSpecial : MonoBehaviour
{
    [HideInInspector] public SilasManager manager;
    private PlayerStateMachine psm;
    private Camera mainCamera;



    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        psm = PlayerID.instance.GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (manager != null)
        {
            if (manager.specialCharges <= 0)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }
    }



    public void StartDash()
    {
        GetComponent<PartyManager>().SetSwappingEnabled(false);
        manager.specialCharges--;
        GameObject bottle = Instantiate(manager.blightPotion, transform.position, transform.rotation);
        bottle.GetComponent<Bottle>().manager = manager;
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = PlayerID.instance.transform.position;
        bottle.GetComponent<Rigidbody2D>().velocity = (mousePos - playerPos).normalized * manager.GetStats().ComputeValue("Special Bomb Speed");

        AudioManager.Instance.SFXBranch.PlaySFXTrack("Silas-Bottle Throw");
        AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Blight Throw");
    }

    public void StopDash()
    {
        GetComponent<PartyManager>().SetSwappingEnabled(true);
    }
}
