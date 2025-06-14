using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles player death logic after health hits zero, including checking for sacrifiices 
/// </summary>
public class PlayerDeathManager : MonoBehaviour
{

    PartyManager party;
    Camera cam;
    Animator camAnim;
    Animator playerAnim;

    [SerializeField] float timescale;
    [SerializeField] float realtimeDuration;

    // Start is called before the first frame update
    void Start()
    {
        party = gameObject.GetComponent<PartyManager>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerAnim = gameObject.GetComponent<Animator>();
    }
    /// <summary>
    /// Starts the player death animation and logic.
    /// Solely called by player Health script once health reaches 0. 
    /// </summary>
    public void PlayDeathAnim()
    {
        // toggle camera zoom in animation 
        Time.timeScale = timescale;
        playerAnim.SetBool("died", true);
        camAnim.SetBool("isDead", true);
        // idk what changing the layer does
        gameObject.layer = 0; // I really hope this doesn't collide with anything
        if (GetComponent<PlayerInput>() != null)
        {
            // disable player input
            GetComponent<PlayerInput>().enabled = false;
        }

        float startTime = Time.unscaledTime;
        float endTime = startTime + realtimeDuration;
        Vector3 originalScale = transform.localScale;

        StartCoroutine(DeathAnimCoroutine(startTime, endTime, originalScale));
    }
    IEnumerator DeathAnimCoroutine(float startTime, float endTime, Vector3 originalScale)
    {
        // perform loop
        while (Time.unscaledTime < endTime)
        {
            float timePercentage = (Time.unscaledTime - startTime) / (endTime - startTime);

            //transform.Rotate(0, 0, Time.unscaledDeltaTime * 360);
            //transform.localScale = originalScale * (1 - timePercentage);

            yield return null;
        }
        // call end of death anim to check for available sacrifices
        EndOfDeathAnim();
    }
    public void EndOfDeathAnim()
    {
        // check if currently selected ghost has sacrifice available
        GhostIdentity curGhost = party.GetSelectedGhost();
        GhostManager curGhostManager = curGhost != null ? curGhost.gameObject.GetComponent<GhostManager>() : null;
        if (curGhostManager && curGhostManager.GetSacrificeReady())
        {
            // activate sacrifice stuff and exit function
            UseSacrifice(curGhostManager);
            return;
        }
        // check down the list of ghosts if their sacrifice is available
        List<GhostIdentity> ghostList = party.GetGhostPartyList();
        foreach (GhostIdentity ghost in ghostList)
        {
            if (curGhost && ghost.name.Equals(curGhost.name))
            {
                // skip this ghost if it is the current ghost (already checked)
                continue;
            }
            GhostManager ghostManager = ghost.gameObject.GetComponent<GhostManager>();
            if (ghostManager.GetSacrificeReady())
            {
                // activate sacrifice stuff and exit function
                UseSacrifice(ghostManager);
                return;
            }
        }

        // if every single ghost in the party doesn't have sacrifice, 
        // reset to hub world and reset everything else we changed in this script
        gameObject.SetActive(false);
        SceneManager.LoadScene("Eva Fractal Hub");
        camAnim.SetBool("isDead", false);
        Time.timeScale = 1;
    }
    /// <summary>
    /// Handles how the sacrifice ability activation call is communicated to the relevant ghost manager.
    /// </summary>
    /// <param name="ghost"></param>
    public void UseSacrifice(GhostManager ghost)
    {
        return;
    }
}
