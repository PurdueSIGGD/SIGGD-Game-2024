using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorManager : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer dotRenderer;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        dotRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = PlayerID.instance.transform.position;
        Vector3 playerToMouseDir = (mousePos - playerPos).normalized;
        float playerToMouseDist = Vector3.Distance(playerPos, mousePos);

        transform.position = mousePos;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, playerPos + (playerToMouseDir * playerToMouseDist * ((float) i / ((float) (lineRenderer.positionCount - 1f)))));
        }
    }
}
