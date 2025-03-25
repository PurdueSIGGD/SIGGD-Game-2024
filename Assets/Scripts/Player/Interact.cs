using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] 
    float interactRadius; // player can only interact with game objects within this radius

    /// <summary>
    /// Input Function - called when player presses button to interact with closest object
    /// </summary>
    public void OnInteract()
    {
        Debug.Log("Interact Input Sensed");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, interactRadius);
        float minDist = Mathf.Infinity;
        IInteractable closest = null;
        foreach (Collider2D collider in colliders)
        {
            IInteractable interactTrigger = collider.gameObject.GetComponent<IInteractable>();
            if (interactTrigger != null)
            {
                float dist = Vector3.Distance(collider.gameObject.transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = interactTrigger;
                }
            }
        }

        if (closest != null) {
            closest.Interact();
        }
    }
}
