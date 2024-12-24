using UnityEngine;

public class PartyPedestal : MonoBehaviour, IInteractable
{
    private static readonly Color INACTIVE_COLOR = Color.gray;
    private static readonly Color ACTIVE_COLOR = Color.green;

    [SerializeField] GameObject menu;

    SpriteRenderer spriteRenderer;

    bool interactable = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = INACTIVE_COLOR;
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.color = ACTIVE_COLOR;
        interactable = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = INACTIVE_COLOR;
        interactable = false;
        menu.SetActive(false);
    }*/

    public void Interact()
    {
        Debug.Log("Party Pedastool Reacted");
        menu.SetActive(true);
    }
}
