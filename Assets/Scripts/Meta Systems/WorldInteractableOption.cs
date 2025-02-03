using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template class for options that are to be displayed on interactable menus.
public abstract class WorldInteractableOption
{
    // Name to be displayed on labels.
    public string name;

    // Whether or not the option should be displayed at all on a menu.
    public bool isVisible;

    // Whether or not the option should be locked/greyed out on a menu.
    public bool isLocked;

    // Callback for the given action.
    public abstract void Action();
}