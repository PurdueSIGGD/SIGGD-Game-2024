using UnityEngine;

//This script allows dampened movement of the camera to follow a target position
public class CameraBuffer : MonoBehaviour
{
    // The buffer's current position
    private Vector2 curPos;
    // The target's position
    private Vector2 targetPos;
    // Keeping track of the initial Z position of the buffer so it doesn't change Z on update
    private float initialZ;

    // Scaling for the buffer's speed
    [SerializeField] private float panSpeedScale;
    // Scales the exponent of the buffer's speed function
    [SerializeField] private float panSpeedExp;
    // The radius limit of the target's distance from the camera center
    [SerializeField] private float radLimit;
    // The object on the parent holding the target's position
    private CameraParentController parentController;
    // The speed at which the buffer travels when the target is at the maximum radius from the center
    private float maxRadSpeed;

    // Start is called before the first frame update
    void Start()
    {
        parentController = this.GetComponentInParent<CameraParentController>();
        curPos = this.gameObject.transform.position;
        initialZ = this.gameObject.transform.position.z;
        maxRadSpeed = panSpeedScale * Mathf.Exp(panSpeedExp*radLimit) - panSpeedScale;
    }

    // LateUpdate is called once per frame (after update)
    // The camera's position is moved
    void FixedUpdate()
    {
        targetPos = parentController.getTarget();
        curPos = this.gameObject.transform.position;
        float dist = Vector2.Distance(curPos, targetPos);
        Vector2 dir = (targetPos - curPos).normalized;
        Vector3 temp;
        if (dist <= radLimit) {
            float speed = panSpeedScale * Mathf.Exp(panSpeedExp*dist) - panSpeedScale;
            temp = curPos + (dir * speed);
        }
        else {
            temp = targetPos - (dir * radLimit) + (dir * maxRadSpeed);
        }
        temp.z = initialZ;
        this.gameObject.transform.position = temp;
    }
}
