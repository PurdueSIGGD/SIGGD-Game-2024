using UnityEngine.UI;
using UnityEngine;

public class PlayerCursorManager : MonoBehaviour
{
    [SerializeField] private Color defaultCursorColor;
    [SerializeField] private Color onHoverCursorColor;

    [SerializeField] private float defaultLength = 2f;
    [SerializeField] private Image cursorIcon;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private RectTransform cursorRect;
    [SerializeField] private LineRenderer defaultLineRenderer;
    [SerializeField] private LineRenderer northLineRenderer;

    private GhostIdentity activeGhost;

    private Camera mainCamera;
    //private SpriteRenderer cursorRenderer;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //cursorRenderer = GetComponent<SpriteRenderer>();
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
        float playerToMouseDist = Vector3.Distance(playerPos, mousePos);

        Vector3 endPos = playerPos + (playerToMouseDir * defaultLength);
        float playerToEndDist = Vector2.Distance(playerPos, endPos);

        //RaycastHit2D hit = Physics2D.Raycast(playerPos, playerToMouseDir, 200f, LayerMask.GetMask("Enemy", "Ground"));
        //Vector3 hitPos = (hit) ? (hit.point) : (playerPos + (playerToMouseDir * 200f));
        //float playerToHitDist = Vector2.Distance(playerPos, hitPos);

        //transform.position = mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 cursorPos
        );
        cursorRect.localPosition = cursorPos;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, playerPos + (playerToMouseDir * playerToEndDist * ((float) i / ((float) (lineRenderer.positionCount - 1f)))));
        }

        /*
        for (int i = 0; i < northLineRenderer.positionCount; i++)
        {
            northLineRenderer.SetPosition(i, playerPos + (playerToMouseDir * playerToHitDist * ((float)i / ((float)(lineRenderer.positionCount - 1f)))));
        }
        */
    }

    public void SetActiveGhost(GhostIdentity activeGhostIdentity)
    {
        activeGhost = activeGhostIdentity;
        cursorIcon.color = (activeGhost == null) ? defaultCursorColor : activeGhost.GetCharacterInfo().primaryColor;
        Color lineColor = (activeGhost == null) ? defaultCursorColor : activeGhost.GetCharacterInfo().primaryColor;
        lineColor = new Color(lineColor.r, lineColor.g, lineColor.b, (150f / 255f));
        lineRenderer.endColor = lineColor;
        lineColor = new Color(lineColor.r, lineColor.g, lineColor.b, 0f);
        lineRenderer.startColor = lineColor;
    }
}
