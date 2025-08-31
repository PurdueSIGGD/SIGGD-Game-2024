using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDocIngredientPickup : MonoBehaviour
{

    private SilasManager manager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeIngredientPickup(SilasManager manager, Vector3 initialVelocity)
    {
        this.manager = manager;
        GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (manager == null) return;
        if (collision.transform.gameObject == PlayerID.instance.gameObject)
        {
            manager.SetIngredientsCollected(manager.ingredientsCollected + 1);
            AudioManager.Instance.SFXBranch.GetSFXTrack("North-Ricochet").SetPitch(Mathf.Max(manager.ingredientsCollected - 1, 0), Mathf.Max(manager.GetStats().ComputeValue("Basic Ingredient Cost") - 1, 0));
            AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Ricochet");
            AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Ingredient");
            if (manager.GetComponent<Bloodletting>().pointIndex > 0) AudioManager.Instance.VABranch.PlayVATrack("Silas-PlagueDoc Bloodletting");
            Destroy(gameObject);
        }
    }
}
