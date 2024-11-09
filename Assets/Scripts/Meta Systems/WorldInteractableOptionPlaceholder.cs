using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableOptionPlaceholder : WorldInteractableOption
{
    public override string GetName()
    {
        return "Placeholder Option";
    }
    public override void Action()
    {
        Debug.Log("Selected Placeholder World Interactable Option");
    }
}
