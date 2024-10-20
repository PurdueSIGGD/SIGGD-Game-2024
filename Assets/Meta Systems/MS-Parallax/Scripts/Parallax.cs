using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Transform camTransform = cam.transform;
        Vector3 position = transform.position;
        Vector3 camPosition = camTransform.position;
        transform.position = new Vector3(camPosition.x * movementFactor, position.y, position.z);
    }
}
