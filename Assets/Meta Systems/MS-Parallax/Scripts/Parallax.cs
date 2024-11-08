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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        width = spriteRenderer.bounds.size.x * 0.5f;
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.size = new Vector2(3.0f, 1.0f);

        startX = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float camX = cam.transform.position.x;

        // Calculate the new position of the object
        float dist = camX * movementFactor;
        float newX = startX + dist;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Calculate left, right bounds of camera and object
        float leftBound = newX - width;
        float rightBound = newX + width;

        float camOrthoWidth = cam.orthographicSize * cam.aspect;
        float leftCamBound = camX - camOrthoWidth;
        float rightCamBound = camX + camOrthoWidth;

        // Tile object horiziontally
        if (leftCamBound >= rightBound) startX += 2 * width;
        if (rightCamBound <= leftBound) startX -= 2 * width;
    }
}
