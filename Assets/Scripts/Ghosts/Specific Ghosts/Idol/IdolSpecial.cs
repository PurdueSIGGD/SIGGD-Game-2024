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
    private PlayerStateMachine psm;
    [SerializeField] private float maxDistance = 8.0f;
    [SerializeField] private float switchCoolDown = 0.5f; // cooldown for switching with clones
    private Camera mainCamera;

    public GameObject idolClone; // ref to clone prefab
    private GameObject swapClone; // ref to currently cloneAliveactive clone, if exists
    public bool spawnSecondClone = false; // used with Dynamic Trio skill
    public bool cloneAlive; // is at least one clone supposed to be alive right now?
    private bool isSwitchOnCooldown = false;

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
            if (manager.getSpecialCooldown() > 0 || isSwitchOnCooldown)
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
            GameplayEventHolder.OnAbilityUsed?.Invoke(manager.onDashContext);
            StartCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// Teleports the player and creates a clone
    /// </summary>
    private IEnumerator DashCoroutine()
    {
        Debug.Log("Holo Dash!");
        Vector3 dest;

        // calculate desired teleport position based on mouse location
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 rawDir = mousePos - pos;

        Debug.Log("mouse:" + mousePos + ", pos:" + pos);

        // Check mouse position for valid teleport position
        dest = mousePos;
        Collider2D[] surfaces = Physics2D.OverlapCircleAll(mousePos, 0.1f, LayerMask.GetMask("Ground"));
        if (surfaces.Length > 0 || Vector2.Distance(mousePos, pos) > manager.GetStats().ComputeValue("HOLOJUMP_MAX_DISTANCE"))
        {
            // cap newpos magnitude at maxDistance
            Vector2 cappedDir = rawDir;
            if (rawDir.magnitude > manager.GetStats().ComputeValue("HOLOJUMP_MAX_DISTANCE"))
            {
                cappedDir = cappedDir.normalized * manager.GetStats().ComputeValue("HOLOJUMP_MAX_DISTANCE");
            }

            Debug.Log("mouse - cappeddir:" + cappedDir);

            // calculate final destination (cannot teleport through "ground" layers)
            RaycastHit2D hit = Physics2D.Raycast(transform.position, cappedDir.normalized, cappedDir.magnitude, LayerMask.GetMask("Ground"));
            if (hit)
            {
                dest = hit.point;
            }
            else
            {
                dest = pos + cappedDir;
            }
        }
        Debug.DrawLine(transform.position, dest);

        // instantiate clone at position (right before teleporting)
        swapClone = Instantiate(idolClone, transform.position, transform.rotation);
        swapClone.GetComponent<IdolClone>().Initialize(
            gameObject,
            manager,
            manager.GetStats().ComputeValue("HOLOJUMP_DURATION_SECONDS"),
            manager.GetStats().ComputeValue("HOLOJUMP_DURATION_INACTIVE_MODIFIER")
        );
        //manager.activeClone = activeClone.GetComponent<IdolClone>();
        cloneAlive = true;
        manager.clones.Add(swapClone);

        // teleport player to final destination
        transform.position = dest;

        // Teleport VFX
        GameObject teleportTracerVFX = Instantiate(manager.holojumpTracerVFX, Vector2.zero, Quaternion.identity);
        teleportTracerVFX.GetComponent<RaycastTracerHandler>().playTracer(swapClone.transform.position, transform.position, true, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        GameObject teleportPulseVfX = Instantiate(manager.holojumpPulseVFX , transform.position, Quaternion.identity);
        teleportPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        GameObject teleportDecoyPulseVfX = Instantiate(manager.holojumpPulseVFX, swapClone.transform.position, Quaternion.identity);
        teleportDecoyPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        
        // create duplicate clone if DYNAMIC TRIO skill says so
        if (spawnSecondClone)
        {
            GameObject secondClone = Instantiate(idolClone, transform.position, transform.rotation);
            secondClone.GetComponent<IdolClone>().Initialize(
                gameObject,
                manager,
                manager.GetStats().ComputeValue("HOLOJUMP_DURATION_SECONDS"),
                manager.GetStats().ComputeValue("HOLOJUMP_DURATION_INACTIVE_MODIFIER")
            );
            manager.clones.Add(secondClone);
        }

        // small pause before player can start swapping with clone
        HoloJumpImmune(manager.GetStats().ComputeValue("HOLOJUMP_IMMUNE_SECONDS"));
        isSwitchOnCooldown = true;
        yield return new WaitForSeconds(manager.GetStats().ComputeValue("HOLOJUMP_CLONE_SWAP_INTERVAL"));
        isSwitchOnCooldown = false;
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

        // Teleport VFX
        GameObject teleportVFX = Instantiate(manager.holojumpTracerVFX, Vector2.zero, Quaternion.identity);
        teleportVFX.GetComponent<RaycastTracerHandler>().playTracer(swapClone.transform.position, transform.position, true, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        GameObject teleportPulseVfX = Instantiate(manager.holojumpPulseVFX, transform.position, Quaternion.identity);
        teleportPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        GameObject teleportDecoyPulseVfX = Instantiate(manager.holojumpPulseVFX, swapClone.transform.position, Quaternion.identity);
        teleportDecoyPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // small pause before player can swap with clone again
        HoloJumpImmune(manager.GetStats().ComputeValue("HOLOJUMP_IMMUNE_SECONDS"));
        isSwitchOnCooldown = true;
        yield return new WaitForSeconds(manager.GetStats().ComputeValue("HOLOJUMP_CLONE_SWAP_INTERVAL"));
        isSwitchOnCooldown = false;
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
            context.damage = 0f;
        }
    }
}