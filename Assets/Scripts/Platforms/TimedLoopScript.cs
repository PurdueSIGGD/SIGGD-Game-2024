using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// General purpose timer script designed for platforms that utilize the ITriggerable interface.
/// </summary>
public class TimedLoopScript : MonoBehaviour
{
    //This is a general timer for platforms that run on a constant, repeating timer


    ITriggerable triggerable; //holds a reference to 

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
            ToggleEffect(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            triggerable.ToggleEffect(!triggerable.GetEffectStatus()); //toggle effect to whatever it was not originally
            if (triggerable.GetEffectStatus())
            {
                timer = onTime;
            }
            else
            {
                timer = offTime;
            }
        }
    }
    /// <summary>
    /// calls the triggerable interface's function of the same ghostName
    /// </summary>
    /// <param ghostName="active"></param>
    void ToggleEffect(bool active)
    {
        triggerable.ToggleEffect(active);
    }
}
