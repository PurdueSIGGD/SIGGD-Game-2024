using UnityEngine;

/// <summary>
/// Handles teleportation between Doors
/// Will teleport player to a sepcified door, if no door is
/// specified in editor, raycast to the right to look for another door,
/// must not have anything in between doors.
/// </summary>
public class Door : MonoBehaviour
{
    public delegate void DoorOpened();
    public static DoorOpened OnDoorOpened;
    public static Door instance;
    [SerializeField] private GameObject dest;
    public static bool active;
    [SerializeField] private Vector3 menuOffset;
    [SerializeField] public bool specificActive;

    private GameObject interactMenu;
    private PlayerID player;
    private SpriteRenderer spriteRenderer;
    protected bool transporting;

    void Start()
    {
        player = PlayerID.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!specificActive && spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        instance = this;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;
        PlayerID.instance.FreezePlayerMouse();

        // disable teleport when door not active
        if (player.CompareTag("Player") && (active || specificActive))
        {
            CreateInteractMenu();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;

        // disable teleport when door not active
        if (interactMenu == null && player.CompareTag("Player") && (active || specificActive))
        {
            CreateInteractMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(interactMenu);
        interactMenu = null;
        PlayerID.instance.UnfreezePlayerMouse();
    }

    // Unlock the door to allow entry to the next room
    public static void activateDoor(bool nactive)
    {
        active = nactive;
        Debug.Log("Open door");
        instance.Activate(nactive);

        if (nactive)
        {
            foreach (DialogueTriggerBox trigger in GameObject.FindObjectsOfType<DialogueTriggerBox>())
            {
                trigger.active = true;
            }
        }
    }

    private void CreateInteractMenu()
    {
        WorldInteract WI = FindAnyObjectByType<WorldInteract>();
        InteractOption opt1 = new InteractOption("Use", CallDoorOpened);//TeleportPlayer);

        Vector3 menuPos = this.transform.position + menuOffset;

        interactMenu = WI.CreateInteractMenu(menuPos, opt1);
        PlayerID.instance.FreezePlayerMouse();
    }

    protected virtual void CallDoorOpened()
    {
        if (!transporting)
        {
            Door.activateDoor(false);
            SendMessage("DoorOpened");
            OnDoorOpened?.Invoke();
            transporting = true;
        }
    }

    public void Activate(bool nbool)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = nbool;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + menuOffset, 0.5f);
    }
}
