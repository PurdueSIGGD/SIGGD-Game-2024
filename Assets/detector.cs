using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detector : MonoBehaviour
{
    //reference to appear disappear
    public appearDisappear appearDisappear;
    public bool isGone = true;
    //change color of sprite reference to sprite
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        appearDisappear = GameObject.FindGameObjectWithTag("platformDisappear").GetComponent<appearDisappear>();
        spriteRenderer = GameObject.FindGameObjectWithTag("buttonSprite").GetComponent<SpriteRenderer>();
        appearDisappear.hidePlatform();
        spriteRenderer.color = Color.red;
    }

    private bool isInsideTrigger = false;

    //basically doing it this way prevents frame skips so everytime ur inside and press i it will trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInsideTrigger = true;
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
            appearDisappear.showPlatform();
            spriteRenderer.color = Color.green;
            isGone = false;
        }
        else if (isInsideTrigger && Input.GetKeyDown(KeyCode.I) && !isGone)
        {
            appearDisappear.hidePlatform();
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
