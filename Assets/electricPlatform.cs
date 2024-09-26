using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricPlatform : MonoBehaviour, ITriggerable
{

    bool isEffectActive;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isEffectActive)
        {
            sprite.color = Color.blue;
        }
        else
        {
            sprite.color = Color.gray;
        }
    }
    public bool toggleEffect(bool active) //ITriggerable method, mutator
    {
        isEffectActive = active;
        return isEffectActive;
    }
    public bool getEffectStatus() //ITriggerable method, accessor
    {
        return isEffectActive;
    }

}
