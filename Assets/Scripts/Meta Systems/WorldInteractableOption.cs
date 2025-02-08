using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Template class for options that are to be displayed on interactable menus.
public abstract class WorldInteractableOption
{
    public abstract string GetName();

    // Whether or not the option should be displayed on the menu.
    public bool isVisible = true;

    // Callback for the button
    protected abstract void Action();

    // Action() is called only if button isVisible
    public void OnClick() {
        if (isVisible) Action();
    }
}