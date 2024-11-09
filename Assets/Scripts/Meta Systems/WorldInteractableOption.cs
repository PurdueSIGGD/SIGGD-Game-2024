using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template class for options that are to be displayed on interactable menus.
public abstract class WorldInteractableOption
{
    // Name to be displayed on labels.
    // Ideally this would be an abstract static member but C# has no such functionality.
    public abstract string GetName();

    // Callback for the given action.
    public abstract void Action();
}