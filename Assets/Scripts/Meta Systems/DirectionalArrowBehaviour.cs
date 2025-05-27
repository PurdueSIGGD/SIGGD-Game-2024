using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowBehaviour : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 intersection;

    [SerializeField] GameObject circle;
    [SerializeField] Vector3 offset;
    [SerializeField] bool blinking;
    [SerializeField] float blinkingOffset;
    [SerializeField] Color color;
    [SerializeField] bool hideOnScreen; // toggling true will ensure the arrow disappears once the entity
                                        // it is pointing to is on screen

    private float blinkingTimer = 0.0f;
    


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        circle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, circle.GetComponent<SpriteRenderer>().color.a);
    }

    // Update is called once per frame
    void Update()
    {
        blinkingTimer += Time.deltaTime;

        if (blinking && blinkingTimer > blinkingOffset)
        {
            circle.SetActive(!circle.activeSelf);
            blinkingTimer = 0.0f;
        }

        float screenHeight = mainCamera.orthographicSize - 0.5f;
        float screenWidth = mainCamera.orthographicSize * Screen.width / Screen.height - 0.5f;
        Vector2 mainCameraPos = mainCamera.transform.position;

        if (Mathf.Abs(transform.position.x - mainCameraPos.x) > screenWidth || Mathf.Abs(transform.position.y - mainCameraPos.y) > screenHeight)
        {
            circle.SetActive(true);
            
            Vector2 a1 = transform.position;
            Vector2 a2 = mainCameraPos;
            if (Intersects(a1, a2, mainCameraPos - new Vector2(screenWidth, screenHeight), mainCameraPos - new Vector2(-screenWidth, screenHeight)))
            {
                circle.transform.position = intersection;
            }else if (Intersects(a1, a2, mainCameraPos - new Vector2(screenWidth, screenHeight), mainCameraPos - new Vector2(screenWidth, -screenHeight)))
            {
                circle.transform.position = intersection;
            }
            else if (Intersects(a1, a2, mainCameraPos - new Vector2(-screenWidth, screenHeight), mainCameraPos - new Vector2(-screenWidth, -screenHeight)))
            {
                circle.transform.position = intersection;
            }
            else if (Intersects(a1, a2, mainCameraPos - new Vector2(screenWidth, -screenHeight), mainCameraPos - new Vector2(-screenWidth, -screenHeight)))
            {
                circle.transform.position = intersection;
            }

            float angle = Mathf.Atan2((a1 - a2).y, (a1 - a2).x);

            circle.transform.rotation = Quaternion.Euler(0, 0, angle * 180/Mathf.PI);
        }
        else
        {
            if (hideOnScreen)
            {
                circle.SetActive(false);
            }
            else
            {
                circle.transform.position = transform.position + offset;
                circle.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        intersection = new Vector2(0, 0);

        Vector2 b = a2 - a1;
        Vector2 d = b2 - b1;
        float bDotDPerp = b.x * d.y - b.y * d.x;

        // if b dot d == 0, it means the lines are parallel so have infinite intersection points
        if (bDotDPerp == 0)
            return false;

        Vector2 c = b1 - a1;
        float t = (c.x * d.y - c.y * d.x) / bDotDPerp;
        if (t < 0 || t > 1)
            return false;

        float u = (c.x * b.y - c.y * b.x) / bDotDPerp;
        if (u < 0 || u > 1)
            return false;

        intersection = a1 + t * b;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
    }

    public void SetColor(Color color)
    {
        circle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, circle.GetComponent<SpriteRenderer>().color.a);
    }

    public void SetBlinking(bool blinking, float blinkingOffset)
    {
        this.blinking = blinking;
        this.blinkingOffset = blinkingOffset;
    }
}
