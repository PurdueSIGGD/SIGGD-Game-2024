using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearDisappear : MonoBehaviour
{
    //reference the object you want to hide or show
    public GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPlatform()
    {
        targetObject.SetActive(true);
    }

    public void hidePlatform()
    { 
        targetObject.SetActive(false);
    }

}
