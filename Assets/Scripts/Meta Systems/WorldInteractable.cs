using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldInteractable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    protected List<WorldInteractableOption> options;

    public abstract void InitializeOptions();
    public abstract void UpdateOptions();

    private void OnEnable()
    {
        options = new List<WorldInteractableOption> ();
        InitializeOptions();
    }

    private void Update()
    {
        UpdateOptions();

        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude < 10)
        {
            foreach (WorldInteractableOption opt in options)
            {
                Debug.Log(opt.GetName());
            }
        }
    }
}
