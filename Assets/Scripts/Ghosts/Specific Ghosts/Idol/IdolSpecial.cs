using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Special action for the idol ghost, teleports the player towards mouse position
/// and leaves a clone that player may swap position with.
/// </summary>
public class IdolSpecial : MonoBehaviour
{
    //bool possessing;
    PlayerStateMachine psm;
    [SerializeField] float maxDistance = 8.0f;
    [SerializeField] float switchCoolDown = 0.5f; // cooldown for switching with clones
    private Camera mainCamera;

    public GameObject idolClone; // ref to clone prefab
    private GameObject swapClone; // ref to currently cloneAliveactive clone, if exists
    public bool spawnSecondClone = false; // used with Dynamic Trio skill
    bool cloneAlive; // is at least one clone supposed to be alive right now?

    [HideInInspector] public IdolManager manager;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        psm = GetComponent<PlayerStateMachine>();
        swapClone = manager.clones.Count > 0 ? manager.clones[0] : null;
        cloneAlive = swapClone != null;
    }

    void Update()
    {
        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }

        // runs once, at the instant the last clone dies

        if (manager.clones.Count == 0 && cloneAlive)
        {
            cloneAlive = false;
            manager.startSpecialCooldown();
        }
    }

    /// <summary>
    /// Swaps position with clone if one is active
    /// If none is active and the skill is not under cooldown, attempt to
    /// teleport the player towards the mouse position.
    /// </summary>
    public void StartHoloJump()
    {
        if (swapClone) // if there is currently a clone active, switch with it
        {
            StartCoroutine(SwitchCoroutine());
        }
        else // else create clone and teleport
        {
            GameplayEventHolder.OnAbilityUsed.Invoke(manager.onDashContext);
            StartCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// Teleports the player and creates a clone
    /// </summary>
    private IEnumerator DashCoroutine()
    {
        Debug.Log("Holo Dash!");

        // calculate desired teleport position based on mouse location

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 rawDir = mousePos - pos;

        Debug.Log("mouse:" + mousePos + ", pos:" + pos);


        // cap newpos magnitude at maxDistance

        Vector2 cappedDir = rawDir;
        if (rawDir.magnitude > manager.GetStats().ComputeValue("HOLOJUMP_MAX_DISTANCE"))
        {
            cappedDir = cappedDir.normalized * manager.GetStats().ComputeValue("HOLOJUMP_MAX_DISTANCE");
        }

        Debug.Log("mouse - cappeddir:" + cappedDir);

        // instantiate clone at position (right before teleporting)

        swapClone = Instantiate(idolClone, transform.position, transform.rotation);
        swapClone.GetComponent<IdolClone>().Initialize(
            gameObject,
            manager.GetStats().ComputeValue("HOLOJUMP_DURATION_SECONDS"),
            manager.GetStats().ComputeValue("HOLOJUMP_DURATION_INACTIVE_MODIFIER"),
            manager
        );
        cloneAlive = true;
        manager.clones.Add(swapClone);

        // calculate final destination (cannot teleport through "ground" layers)

        RaycastHit2D hit = Physics2D.Raycast(transform.position, cappedDir.normalized, cappedDir.magnitude, LayerMask.GetMask("Ground"));
        Vector3 dest;
        if (hit)
        {
            dest = hit.point;
        }
        else
        {
            dest = pos + cappedDir;
        }
        Debug.DrawLine(transform.position, dest);

        // teleport player to final destination

        transform.position = dest;

        // create duplicate clone if DYNAMIC TRIO skill says so

        if (spawnSecondClone)
        {
            GameObject secondClone = Instantiate(idolClone, transform.position, transform.rotation);
            secondClone.GetComponent<IdolClone>().Initialize(
                gameObject,
                manager.GetStats().ComputeValue("HOLOJUMP_DURATION_SECONDS"),
                manager.GetStats().ComputeValue("HOLOJUMP_DURATION_INACTIVE_MODIFIER"),
                manager
            );
            manager.clones.Add(secondClone);
        }

        // small pause before player can start swapping with clone

        HoloJumpImmune(manager.GetStats().ComputeValue("HOLOJUMP_IMMUNE_SECONDS"));
        yield return new WaitForSeconds(manager.GetStats().ComputeValue("HOLOJUMP_CLONE_SWAP_INTERVAL"));
        psm.EnableTrigger("OPT");
    }

    /// <summary>
    /// Switch position with clone
    /// </summary>
    private IEnumerator SwitchCoroutine()
    {
        // check if clone still exists

        if (swapClone == null)
        {
            if (manager.clones.Count == 0)
            {
                yield break;
            }
            swapClone = manager.clones[0];
        }
        Debug.Log("Holo Swap!");

        // switch position with active clone

        (psm.transform.position, swapClone.transform.position) = (swapClone.transform.position, psm.transform.position);

        // small pause before player can swap with clone again

        HoloJumpImmune(manager.GetStats().ComputeValue("HOLOJUMP_IMMUNE_SECONDS"));
        yield return new WaitForSeconds(manager.GetStats().ComputeValue("HOLOJUMP_CLONE_SWAP_INTERVAL"));
        psm.EnableTrigger("OPT");
    }

    /// <summary>
    /// Returns a ref to the currently active clone, may not exist
    /// </summary>
    public GameObject GetClone()
    {
        return swapClone;
    }
    public void RemoveCloneFromList(GameObject clone)
    {
        manager.clones.Remove(clone);
    }
    void HoloJumpImmune(float time)
    {
        StartCoroutine(ImmuneTimer(time));
    }

    IEnumerator ImmuneTimer(float time)
    {
        GameplayEventHolder.OnDamageFilter.Add(HoloJumpImmuneFilter);
        yield return new WaitForSeconds(time);
        GameplayEventHolder.OnDamageFilter.Remove(HoloJumpImmuneFilter);
    }

    void HoloJumpImmuneFilter(ref DamageContext context)
    {
        if (context.victim.CompareTag("Player"))
        {
            context.damage = 0;
        }
    }
}