using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script sets a target coordinate position for the orthographic camera
public class CameraParentController : MonoBehaviour
{

    // Takes the main camera of the scene
    [SerializeField] private Camera mainCamera;

    // Takes the transform of the player
    [SerializeField] private Transform playerTransform;

    // Offset of the home position from the player position
    [SerializeField] private Vector2 targetOffset;

    // Scales the amount of aim pull that affects the camera
    [SerializeField] private float aimPullStrength = 0.1f;

    // The lateral distance from the player at which aim pull takes effect
    [SerializeField] private float aimPullXDistance = 10f;

    // The vertical distance from the player at which aim pull takes effect
    [SerializeField] private float aimPullYDistance = 5f;

    // A vector 3 position that represents what the center
    // of the orthographic camera should focus on.  
    // The z coordinate is ignored.  
    private Vector3 cameraTarget;

    private bool isAimPulling = false;





    // Start is called before the first frame update
    void Start()
    {
        cameraTarget = this.gameObject.transform.position;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform != null) {
            // Get mouse and screen center world position and standard home position
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 screenCenterPosition = mainCamera.transform.position;
            screenCenterPosition.z = 0f;
            Vector3 homePosition = new Vector3(playerTransform.position.x + targetOffset.x, playerTransform.position.y + targetOffset.y, 0f);
            Vector3 aimingDirection = mousePosition - screenCenterPosition;

            // Determine whether aim pull is needed
            bool isAimingWide = (Mathf.Abs(aimingDirection.x) >= aimPullXDistance ||
                                 Mathf.Abs(aimingDirection.y) >= aimPullYDistance);
            bool isAimingClose = (aimingDirection.magnitude < aimPullYDistance);
            if (!isAimPulling && isAimingWide)
            {
                isAimPulling = true;
            }
            if (isAimPulling && isAimingClose)
            {
                isAimPulling = false;
            }

            // Set cameraTarget
            cameraTarget = homePosition;
            cameraTarget += (isAimPulling) ? (aimingDirection * aimPullStrength) : Vector3.zero;
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
