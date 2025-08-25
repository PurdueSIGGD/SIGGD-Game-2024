using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefAmmoPickup : MonoBehaviour
{

    private PoliceChiefManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeAmmoPickup(PoliceChiefManager manager, Vector3 initialVelocity)
    {
        this.manager = manager;
        GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (manager == null) return;
        if (collision.transform.gameObject == PlayerID.instance.gameObject)
        {
            manager.basic.AddAmmo(1);
            AudioManager.Instance.SFXBranch.GetSFXTrack("North-Ricochet").SetPitch(Mathf.Max(manager.basicAmmo - 1, 0), Mathf.Max(manager.GetStats().ComputeValue("Basic Starting Ammo") - 1, 0));
            AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Ricochet");
            Destroy(gameObject);
        }
    }
}
