using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldInteractable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float activationRange;

    protected List<WorldInteractableOption> options;

    // These two methods are to be implemented by each specific menu instance.
    public abstract void InitializeOptions();
    public abstract void UpdateOptions();

    private void OnEnable()
    {
        options = new List<WorldInteractableOption> ();
        InitializeOptions();
    }

    // Update function checks if player is in range, then updates and displays the options list.
    private void Update()
    {
        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange) return;
        
        UpdateOptions();
        foreach (WorldInteractableOption opt in options)
        {
            Debug.Log(opt.GetName());
        }
    }
}
