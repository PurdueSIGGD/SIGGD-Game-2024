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

    [SerializeField] private GameObject dest;
    public static bool active;
    [SerializeField] private Vector3 menuOffset;
    [SerializeField] private bool specificActive;

    private GameObject interactMenu;
    private PlayerID player;


    void Start()
    {
        player = PlayerID.instance;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;

        // disable teleport when door not active
        if (player.CompareTag("Player") && (active || specificActive))
        {
            CreateInteractMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(interactMenu);
        interactMenu = null;
    }

    // Unlock the door to allow entry to the next room
    public static void activateDoor(bool nactive)
    {
        active = nactive;
    }

    private void CreateInteractMenu()
    {
        WorldInteract WI = FindAnyObjectByType<WorldInteract>();
        InteractOption opt1 = new InteractOption("Use", CallDoorOpened);//TeleportPlayer);

        Vector3 menuPos = this.transform.position + menuOffset;

        interactMenu = WI.CreateInteractMenu(menuPos, opt1);
    }

    private void CallDoorOpened()
    {
        Door.activateDoor(false);
        SendMessage("DoorOpened");
        OnDoorOpened();
    }

    private void TeleportPlayer()
    {
        RaycastHit2D hit;
        Vector2 pos = new Vector2(0, 0);

        if (dest)
        {
            pos = dest.transform.position;
        }
        else
        {
            // if no pre-determined destination, raycast to the right to find the closest
            // door as destination
            Vector3 rayOrig = new Vector3(transform.position.x + transform.lossyScale.x,
                                          transform.position.y, transform.position.z);
            hit = Physics2D.Raycast(rayOrig, transform.right, Mathf.Infinity);
            if (hit)
            {
                pos = hit.transform.position;
            }
            else
            {
                Debug.LogWarning(gameObject.name + " cannot find suitable destination");
            }
        }
        // raycast down so the player is spawned on the floor
        hit = Physics2D.Raycast(pos, -transform.up, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit)
        {
            player.transform.position = hit.point;
        }
        else
        {
            Debug.LogWarning("Please ensure " + gameObject.name + " is placed over a platform");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + menuOffset, 0.5f);
    }
}
