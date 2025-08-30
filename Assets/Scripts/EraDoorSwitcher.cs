using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraDoorSwitcher : MonoBehaviour
{
    [SerializeField] int orionNumber;
    [SerializeField] Sprite incomplete;
    [SerializeField] Sprite complete;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject baseToAppear;

    // Start is called before the first frame update
    void Update()
    {
        if (SaveManager.data.orion + 1 < orionNumber)
        {
            if (baseToAppear != null)
            {
                baseToAppear.SetActive(false);
            }
            GetComponent<Door>().specificActive = false;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            //gameObject.SetActive(false);
            //spriteRenderer.enabled = false;
        }
        else if (SaveManager.data.orion >= orionNumber)
        {
            if (baseToAppear != null)
            {
                baseToAppear.SetActive(true);
            }
            GetComponent<Door>().specificActive = true;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            gameObject.SetActive(true);
            //spriteRenderer.enabled = true;
            spriteRenderer.sprite = complete;
        }
        else
        {
            if (baseToAppear != null)
            {
                baseToAppear.SetActive(true);
            }
            GetComponent<Door>().specificActive = true;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            gameObject.SetActive(true);
            //spriteRenderer.enabled = true;
            spriteRenderer.sprite = incomplete;
        }
    }
}
