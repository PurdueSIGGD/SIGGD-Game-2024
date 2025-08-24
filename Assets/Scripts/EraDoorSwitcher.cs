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
        if (SaveManager.data.orion - 1 < orionNumber)
        {
            spriteRenderer.enabled = false;
        }
        if (SaveManager.data.orion >= orionNumber)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = complete;
        }
        else
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = incomplete;
        }
    }
}
