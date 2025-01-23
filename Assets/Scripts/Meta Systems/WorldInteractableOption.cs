using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template class for options that are to be displayed on interactable menus.
public abstract class WorldInteractableOption
{
    // Name to be displayed on labels.
    // Ideally this would be an abstract static member but C# has no such functionality.
    public abstract string GetName();

    // Checks and returns whether or not the option should be displayed at all on a menu.
    public abstract bool IsVisible();

    // Checks and returns whether or not the option should be locked/greyed out on a menu.
    public abstract bool IsLocked();

    // Callback for the given action.
    public abstract void Action();
}