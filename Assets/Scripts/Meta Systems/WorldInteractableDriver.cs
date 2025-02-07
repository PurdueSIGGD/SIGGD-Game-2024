using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableDriver : MonoBehaviour
{
    [SerializeField] WorldInteractable menu;
    private void Start()
    {
        List<WorldInteractableOption> opts = new List<WorldInteractableOption> ();
        WorldInteractableOptionPlaceholder opt1 = new WorldInteractableOptionPlaceholder ();
        WorldInteractableOptionPlaceholder opt2 = new WorldInteractableOptionPlaceholder ();
        WorldInteractableOptionPlaceholder opt3 = new WorldInteractableOptionPlaceholder ();

        menu.InstantiateButtons(opts);
    }
}