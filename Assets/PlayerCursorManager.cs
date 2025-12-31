using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = PlayerID.instance.transform.position;
        Vector3 playerToMouseDir = Vector3.Normalize(mousePos - playerPos);

        RaycastHit2D hit = Physics2D.Raycast(playerPos, playerToMouseDir, 50f, LayerMask.GetMask("Enemy", "Ground"));
        Vector3 hitPos = (hit) ? (hit.point) : (mousePos); //(playerPos + (playerToMouseDir * 6f));
        float playerToHitDist = Vector2.Distance(playerPos, hitPos);

        float playerToMouseDist = Vector3.Distance(playerPos, mousePos);

        //transform.position = mousePos;
        //transform.position = hitPos;
        transform.position = playerPos + (playerToMouseDir * playerToMouseDist);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, playerPos + (playerToMouseDir * playerToHitDist * ((float) i / ((float) (lineRenderer.positionCount - 1f)))));
        }
    }
}
