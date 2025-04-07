using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlightParticles : MonoBehaviour
{
    [SerializeField] private GameObject blightPrefab;
    private HashSet<int> uniqueHits;

    void Start()
    {
        uniqueHits = new HashSet<int>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Health>() == null) return; // do not try to add blight to obj without health

        if (!uniqueHits.Contains(other.GetInstanceID()))
        {
            uniqueHits.Add(other.GetInstanceID());
            if (other.GetComponentInChildren<Blight>() == null)
            {
                GameObject blight = Instantiate(blightPrefab, other.transform);
                blight.GetComponent<Blight>().effectParent = other;
            }
        }
    }
}
