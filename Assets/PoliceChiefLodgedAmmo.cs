using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PoliceChiefLodgedAmmo : MonoBehaviour
{

    private int ammoLodged = 0;
    private GameObject directionalIndicator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeDirectionalIndicator(GameObject directionalIndicator)
    {
        this.directionalIndicator = Instantiate(directionalIndicator, transform);
        this.directionalIndicator.SetActive(false);
    }

    public void SetAmmoLodged(int ammoLodged)
    {
        this.ammoLodged = Mathf.Max(ammoLodged, 0);
        directionalIndicator.SetActive(this.ammoLodged > 0);
    }

    public int GetAmmoLodged()
    {
        return this.ammoLodged;
    }
}
