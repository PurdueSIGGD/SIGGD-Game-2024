using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCameraInsidePalace : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    private void LateUpdate()
    {
        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float y = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
