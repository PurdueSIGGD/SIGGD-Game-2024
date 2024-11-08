using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private bool tiling;
    [SerializeField] private float movementFactor; // Speed of object relative to camera speed (Between 0.0f - 1.0f)

    private float startX;
    private float width;
    private float camWidth;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        width = spriteRenderer.bounds.size.x * 0.5f;
        Assert.IsTrue(cam.orthographic == true);
        camWidth = cam.orthographicSize * cam.aspect; // This assumes camera is orthographic!

        // Tiled such that there are 3 copies of the sprite, one in the middle and one to either side
        if (tiling) {
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new Vector2(3.0f, 1.0f);
        }

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

        if (tiling) {
            // Calculate left, right bounds of camera and object
            float leftBound = newX - width;
            float rightBound = newX + width;

            float leftCamBound = camX - camWidth;
            float rightCamBound = camX + camWidth;

            // Tile object horiziontally
            if (leftCamBound >= rightBound) startX += 2 * width;
            if (rightCamBound <= leftBound) startX -= 2 * width;
        }

    }
}
