using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldInteractableOptionPlaceholder : WorldInteractableOption{
    public override string GetName()
    {
        return "Placeholder";
    }
    protected override void Action() {
        Debug.Log("Hello!");
    }
} 