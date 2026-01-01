using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCursorManager : MonoBehaviour
{
    [SerializeField] private LineRenderer defaultLineRenderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] private float defaultLength = 2f;

    private Camera mainCamera;
    private SpriteRenderer dotRenderer;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        dotRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = defaultLineRenderer;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0f);
        Vector3 playerPos = PlayerID.instance.transform.position;
        Vector3 playerToMouseDir = Vector3.Normalize(mousePos - playerPos);

        //RaycastHit2D hit = Physics2D.Raycast(playerPos, playerToMouseDir, directionalIndicatorLength, LayerMask.GetMask("Enemy", "Ground"));
        //Vector3 hitPos = (hit) ? (hit.point) : (playerPos + (playerToMouseDir * directionalIndicatorLength));
        Vector3 hitPos = playerPos + (playerToMouseDir * defaultLength);
        float playerToHitDist = Vector2.Distance(playerPos, hitPos);

        float playerToMouseDist = Vector3.Distance(playerPos, mousePos);

        transform.position = mousePos;
        //transform.position = hitPos;
        //transform.position = playerPos + (playerToMouseDir * playerToMouseDist);
        
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, playerPos + (playerToMouseDir * playerToHitDist * ((float) i / ((float) (lineRenderer.positionCount - 1f)))));
        }
        
    }
}
