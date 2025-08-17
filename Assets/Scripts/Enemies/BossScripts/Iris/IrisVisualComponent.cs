using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisVisualComponent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetSpriteState(int damageState)
    {
        if (damageState >= 0 && damageState < sprites.Count)
            spriteRenderer.sprite = sprites[damageState];
    }
}
