using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will be stored in a parent manager class
public class WorldInteractable
{
    [SerializeField] private Vector3 position;
    private List<WorldInteractableOption> options;
    public WorldInteractable(List<WorldInteractableOption> opt)
    {
        options = opt;
    }

    public void Update(Vector3 playerPosition)
    {
        Vector3 dist = playerPosition - position;
        if (dist.magnitude < 10)
        {
            foreach (WorldInteractableOption option in options)
            {
                Debug.Log(option.GetName());
            }
        }
    }
}
