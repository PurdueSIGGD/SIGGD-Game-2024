using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDisappear : MonoBehaviour
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

    public void ShowPlatform()
    {
        targetObject.SetActive(true);
    }

    public void HidePlatform()
    {
        targetObject.SetActive(false);
    }

}
