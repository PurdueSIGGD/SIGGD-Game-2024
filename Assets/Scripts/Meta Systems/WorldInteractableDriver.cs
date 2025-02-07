using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableDriver : MonoBehaviour
{
    [SerializeField] WorldInteractable menu;
    private void Start()
    {
        List<WorldInteractableOption> opts = new List<WorldInteractableOption> () {
            new WorldInteractableOptionPlaceholder (),
            new WorldInteractableOptionPlaceholder (),
            new WorldInteractableOptionPlaceholder (),
        };
        menu.InstantiateButtons(opts);
        menu.UpdateButtons();
    }
}