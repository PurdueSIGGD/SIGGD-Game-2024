using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float movementFactor;

    private float startX;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float camX = cam.transform.position.x;

        float dist = camX * movementFactor;
        float temp = camX * (1-  movementFactor);

        transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);

        if (temp > startX + width)
        {
            startX += width;
        }
        else if (temp < startX - width)
        {
            startX -= width;
        }
    }
}
