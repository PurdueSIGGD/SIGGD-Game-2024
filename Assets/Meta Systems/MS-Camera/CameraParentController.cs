using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script sets a target coordinate position for the orthographic camera
public class CameraParentController : MonoBehaviour
{
    // A vector 3 position that represents what the center
    // of the orthographic camera should focus on.  
    // The z coordinate is ignored.  
    private Vector3 cameraTarget;

    // Testing field, takes the transform of a test target
    [SerializeField] private Transform testTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTarget = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // This is only present for testing
        if (testTransform != null) {
            cameraTarget = testTransform.position;
        }
    }

    /// <summary>
    /// Returns the camera's target position
    /// </summary>
    /// <returns></returns>
    public Vector3 getTarget() {
        return cameraTarget;
    }

    /// <summary>
    /// Sets the camera's target position
    /// </summary>
    /// <param ghostName="targ">the target position</param>
    public void setTarget(Vector3 targ) {
        cameraTarget = targ;
    }

}
