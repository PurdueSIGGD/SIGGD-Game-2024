using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricPlatform : MonoBehaviour, ITriggerable
{

    bool isEffectActive; //is the special platform effect on or off?
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //sprite to change color to indicate effect on or off, because player hurt implementation by player team not done yet
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
