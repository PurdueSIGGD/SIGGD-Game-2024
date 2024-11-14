using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractablePlaceholder : WorldInteractable
{
    public override void InitializeOptions()
    {
        options.Add(new WorldInteractableOptionPlaceholder());
        options.Add(new WorldInteractableOptionPlaceholder());
        options.Add(new WorldInteractableOptionPlaceholder());
    }

    public override void UpdateOptions()
    {

    }
}