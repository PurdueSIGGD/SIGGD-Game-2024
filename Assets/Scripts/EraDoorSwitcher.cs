using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraDoorSwitcher : MonoBehaviour
{
    [SerializeField] int orionNumber;
    [SerializeField] Sprite incomplete;
    [SerializeField] Sprite complete;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.data.orion + 1 < orionNumber)
        {
            gameObject.SetActive(false);
            //spriteRenderer.enabled = false;
        }
        else if (SaveManager.data.orion >= orionNumber)
        {
            gameObject.SetActive(true);
            //spriteRenderer.enabled = true;
            spriteRenderer.sprite = complete;
        }
        else
        {
            gameObject.SetActive(true);
            //spriteRenderer.enabled = true;
            spriteRenderer.sprite = incomplete;
        }
    }
}
