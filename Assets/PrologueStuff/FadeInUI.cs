using TMPro;
using UnityEngine;

public class FadeInUI : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] float fadeInSpeed;
    [SerializeField] float fadeOutSpeed;
    [SerializeField] TextMeshPro text;
    
    private Color c;
    private bool colliding;
    private bool disenabling;

    void Start()
    {
        fadeInSpeed = fadeInSpeed / 100;
        fadeOutSpeed = fadeOutSpeed / 100;
        c = text.color;
        c.a = 0;
        text.color = c;
    }

    void Update()
    {
        if (disenabling)
        {
            if (c.a <= 0) Destroy(gameObject);
            c.a = Mathf.Clamp(c.a - Time.deltaTime * fadeOutSpeed, 0, 1);
            text.color = c;
            return;
        }

        if (!active) return;

        if (colliding)
        {
            c.a = Mathf.Clamp(c.a + Time.deltaTime * fadeInSpeed, 0 , 1);
        }
        else
        {
            c.a = Mathf.Clamp(c.a - Time.deltaTime * fadeOutSpeed, 0, 1);
        }
        text.color = c;
    }

    public void EnableFadeIn(bool active)
    {
        this.active = active;
    }

    public void DisenablePrompt()
    {
        disenabling = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerID.instance.gameObject)
        {
            colliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerID.instance.gameObject)
        {
            colliding = false;
        }
    }
}
