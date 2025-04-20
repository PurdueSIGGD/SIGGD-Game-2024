using UnityEngine;

public class PartyPedestal : MonoBehaviour, IInteractable
{
    private static readonly Color INACTIVE_COLOR = Color.gray;
    private static readonly Color ACTIVE_COLOR = Color.green;

    [SerializeField] GameObject menu;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = INACTIVE_COLOR;
    }

    public void Interact()
    {
        Debug.Log("Party Pedastool Reacted");
        menu.SetActive(true);
    }
}
