using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Special action for the idol ghost, teleports the player towards mouse position
/// and leaves a clone that player may swap position with.
/// </summary>
public class IdolSpecial : MonoBehaviour, IParty, ISelectable
{
    bool possessing;
    PlayerStateMachine psm;
    [SerializeField] float maxDistance = 8.0f;
    [SerializeField] float tpCoolDown = 8.0f; // cooldown for teleporting special action
    [SerializeField] float switchCoolDown = 0.5f; // cooldown for switching with clones

    private bool isDashing; // bool for if is currently dashing
    private bool canTp = true;
    private bool canSwitch = false;
    private Camera mainCamera;
    private Vector2 dir;

    private GameObject idolClone; // ref to clone prefab
    private GameObject activeClone; // ref to currently active clone, if exists

    public void EnterParty(GameObject player)
    {
        return;
    }
    public void ExitParty(GameObject player)
    {
        return;
    }
    public void Select(GameObject player)
    {
        possessing = true;
        psm = player.GetComponent<PlayerStateMachine>();

        // override delegate
        player.GetComponent<Dash>().specialAction = HoloJump;
    }
    public void DeSelect(GameObject player)
    {
        possessing = false;

        // unoverride delegate
        player.GetComponent<Dash>().specialAction = null;
    }

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        idolClone = Resources.Load<GameObject>("IdolClone");
    }

    /// <summary>
    /// Swaps position with clone if one is active
    /// If none is active and the skill is not under cooldown, attempt to
    /// teleport the player towards the mouse position.
    /// </summary>
    void HoloJump()
    {
        if (canSwitch) // if there is currently a clone active, switch with it
        {
            StartCoroutine(SwitchCoroutine());
        }
        else if (canTp) // else create clone and teleport
        {
            StartCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// Teleports the player and creates a clone
    /// </summary>
    private IEnumerator DashCoroutine()
    {
        print("Holo Dash!");

        // instant cast time
        psm.EnableTrigger("OPT");

        isDashing = true;
        canTp = false;
        dir = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // create a clone
        activeClone = Instantiate(idolClone, transform.position, transform.rotation);
        activeClone.GetComponent<IdolClone>().Initialize(gameObject);
        canSwitch = true;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Ground"));
        Vector3 dest;
        if (hit)
        {
            dest = hit.point;
        }
        else
        {
            float destX = transform.position.x + (dir * maxDistance).x;
            float destY = transform.position.y + (dir * maxDistance).y;
            dest = new Vector2(destX, destY);
        }
        transform.position = dest;
        isDashing = false;

        // do not use psm cooldown because of swap mechanic
        // psm.OnCooldown("c_special");
        yield return new WaitForSeconds(tpCoolDown);
        // psm.OffCooldown("c_special");

        canTp = true;
    }

    /// <summary>
    /// Switch position with clone
    /// </summary>
    private IEnumerator SwitchCoroutine()
    {
        canSwitch = false;
        // check if clone still exists
        if (activeClone == null)
        {
            // if not, dash instead
            StartCoroutine(DashCoroutine());
        }
        else
        {
            print("Holo Swap!");

            // instant cast time
            psm.EnableTrigger("OPT");

            // if so, switch places with clone
            (transform.position, activeClone.transform.position) = (activeClone.transform.position, transform.position);
            psm.OnCooldown("c_special");
            yield return new WaitForSeconds(switchCoolDown);
            psm.OffCooldown("c_special");
            canSwitch = true;
        }
    }
}
