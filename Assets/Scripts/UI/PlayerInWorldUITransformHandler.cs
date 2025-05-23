using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInWorldUITransformHandler : MonoBehaviour
{
    private Quaternion UIRotation;
    private Vector3 UIScale;
    private float UIScaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        UIRotation = new Quaternion(0f, transform.parent.rotation.y, 0f, 0f);
        UIScale = new Vector3(transform.parent.localScale.x, transform.localScale.y, transform.localScale.z);
        UIScaleFactor = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        UIRotation.y = transform.parent.rotation.y;
        transform.localRotation = UIRotation;
        UIScale.x = (transform.parent.localScale.x / Mathf.Abs(transform.parent.localScale.x)) * UIScaleFactor;
        transform.localScale = UIScale;
    }
}
