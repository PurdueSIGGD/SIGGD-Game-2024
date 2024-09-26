using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class timedLoopPlatform : MonoBehaviour
{
    //This is a general timer for platforms that run on a constant, repeating timer


    ITriggerable triggerable;

    [SerializeField] float offTime; //amount of time in seconds that the effect is off
    [SerializeField] float onTime; //amount of time in seconds that the effect is on
    [SerializeField] bool startOn; //should the effect start on or off when it is loaded? 
    [SerializeField] float timer; //timer variable to do the actual counting


    // Start is called before the first frame update
    void Start()
    {
        triggerable = this.GetComponent<ITriggerable>(); // set ITriggerable interface reference to whatever script uses ITriggerable in the platform game object 
        if (triggerable == null)
        {
            print("ERROR: timed platform script couldn't find any ITriggerable component");
        }

        timer = offTime;
        //object will be off by default, if startOn is true make object effect on
        if (startOn)
        {
            timer = onTime;
            toggleEffect(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            triggerable.toggleEffect(!triggerable.getEffectStatus()); //toggle effect to whatever it was not originally
            if (triggerable.getEffectStatus())
            {
                timer = onTime;
            }
            else
            {
                timer = offTime;
            }
        }
    }
    void toggleEffect(bool active)
    {
        triggerable.toggleEffect(active);
    }
}
