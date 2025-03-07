using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float interactRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
