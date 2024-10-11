using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //reference to appear disappear
    public AppearDisappear appearDisappear;
    public bool isGone = true;
    //change color of sprite reference to sprite
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        appearDisappear = GameObject.FindGameObjectWithTag("platformDisappear").GetComponent<AppearDisappear>();
        spriteRenderer = GameObject.FindGameObjectWithTag("buttonSprite").GetComponent<SpriteRenderer>();
        appearDisappear.HidePlatform();
        spriteRenderer.color = Color.red;
    }

    private bool isInsideTrigger = false;

    //basically doing it this way prevents frame skips so everytime ur inside and press i it will trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInsideTrigger = true;
        print("collision");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInsideTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInsideTrigger && Input.GetKeyDown(KeyCode.I) && isGone)
        {
            appearDisappear.ShowPlatform();
            spriteRenderer.color = Color.green;
            isGone = false;
        }
        else if (isInsideTrigger && Input.GetKeyDown(KeyCode.I) && !isGone)
        {
            appearDisappear.HidePlatform();
            spriteRenderer.color = Color.red;
            isGone = true;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{ 
    //    //this is where you would add script for "turning on button"
    //    if (Input.GetKeyDown(KeyCode.I)) 
    //    {
    //        appearDisappear.hidePlatform();
    //    }
    //}
}
