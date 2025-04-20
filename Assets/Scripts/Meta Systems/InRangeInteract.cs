using UnityEngine;

public abstract class InRangeInteract : MonoBehaviour
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [Header("Parent - InRangeInteract")]

    [SerializeField]
    private float interactRange;

    [SerializeField]
    private Vector3 menuOffset;

    // ==============================
    //        Other Variables
    // ==============================

    private GameObject interactMenu;
    private WorldInteract WI;
    private Vector3 lastPos;

    // ==============================
    //        Unity Functions
    // ==============================

    private void Start()
    {
        WI = FindAnyObjectByType<WorldInteract>();
        lastPos = PlayerID.instance.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 currPos = PlayerID.instance.transform.position;
        float delta = Vector3.Distance(lastPos, currPos);
        if (delta > 0.001f)
        {
            CheckMenu();
        }
        lastPos = currPos;
    }

    // ==============================
    //       Private Functions
    // ==============================

    private bool PlayerInRange()
    {
        float dist = Vector3.Distance(PlayerID.instance.transform.position, this.transform.position);

        return (dist < interactRange);
    }

    private void CheckMenu()
    {
        if (PlayerInRange() && interactMenu == null)
        {
            OpenMenu();
        }
        else if (!PlayerInRange() && interactMenu != null)
        {
            CloseMenu();
        }
    }

    private void OpenMenu()
    {
        Vector3 menuPos = this.transform.position + menuOffset;
        interactMenu = WI.CreateInteractMenu(menuPos, GetMenuOptions());
        PlayerID.instance.FreezePlayerMouse();
    }
    // ==============================
    //        Other Functions
    // ==============================

    protected abstract InteractOption[] GetMenuOptions();

    protected void CloseMenu()
    {
        Destroy(interactMenu);
        interactMenu = null;
        PlayerID.instance.UnfreezePlayerMouse();
    }
}
