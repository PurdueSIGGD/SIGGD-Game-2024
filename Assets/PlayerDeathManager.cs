using System;
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
    [SerializeField]
    string respawnScene = "Hubworld";

    float endTime;
    bool animRunning;


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
        endTime = Time.unscaledTime + realtimeDuration;
        animRunning = true;
    }
    public void Update()
    {
        if (animRunning)
        {
            // perform loop
            if (Time.unscaledTime >= endTime)
            {
                // call end of death anim to check for available sacrifices
                animRunning = false;
                EndOfDeathAnim();
            }
        }
    }
    public void EndOfDeathAnim()
    {
        bool didSacrifice = false;

        // check if currently selected ghost has sacrifice available
        GhostIdentity curGhost = party.GetSelectedGhost();
        GhostManager curGhostManager = curGhost != null ? curGhost.gameObject.GetComponent<GhostManager>() : null;
        if (curGhostManager && curGhostManager.GetSacrificeReady())
        {
            // activate sacrifice stuff and exit function
            UseSacrifice(curGhostManager);
            didSacrifice = true;
        }
        // check down the list of ghosts if their sacrifice is available
        List<GhostIdentity> ghostList = party.GetGhostPartyList();
        foreach (GhostIdentity ghost in ghostList)
        {
            if (didSacrifice)
            {
                break;
            }
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
                didSacrifice = true;
            }
        }
        camAnim.SetBool("isDead", false);
        Time.timeScale = 1;

        // if every single ghost in the party doesn't have sacrifice, 
        // reset to hub world and reset everything else we changed in this script
        if (!didSacrifice)
        {
            // update death story progression if first death
            if (SaveManager.data.death == 0)
            {
                SaveManager.data.death = 1;
                SaveManager.instance.Save();
            }
            // make the player die for real if no sacrifices occured here
            gameObject.SetActive(false);
            SceneManager.LoadScene(respawnScene);
        }

    }
    /// <summary>
    /// Handles how the sacrifice ability activation call is communicated to the relevant ghost manager.
    /// </summary>
    /// <param name="ghost"></param>
    public void UseSacrifice(GhostManager ghost)
    {
        // playerAnim.SetBool("died", false);
        // camAnim.SetBool("isDead", false);
        // Time.timeScale = 1;
        print("AND THEY SACRIFICE... THE GHOOOST!!!");
        //
        // TODO: IMPLEMENT SACRIFICE CALL THROUGH GHOST MANAGER HERE!!!
        //
        return;
    }
}
