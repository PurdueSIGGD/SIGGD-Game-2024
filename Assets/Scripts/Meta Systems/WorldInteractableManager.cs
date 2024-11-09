using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractableManager : MonoBehaviour
{
    private List<WorldInteractable> interactables = new List<WorldInteractable>();
    private void OnEnable()
    {
        // Implement all interactables and their options

        // Interactable 1
        List<WorldInteractableOption> interactableList1 = new List<WorldInteractableOption>();
        interactableList1.Add(new WorldInteractableOptionPlaceholder());
        interactableList1.Add(new WorldInteractableOptionPlaceholder());
        interactableList1.Add(new WorldInteractableOptionPlaceholder());
        WorldInteractable worldInteractable1 = new WorldInteractable(interactableList1);
        interactables.Add(worldInteractable1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (WorldInteractable interactable in interactables)
        {
            interactable.Update(Vector3.zero);
        }
    }
}
