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
    [SerializeField] float realtimeFadeOutStart;
    [SerializeField] float realtimeFadeOutDuration;
    [SerializeField] float realtimeDeathRingStart;
    [SerializeField]
    string respawnScene = "Hubworld";

    float endTime;
    float fadeOutTime;
    bool isFadingOut;
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

        playerAnim.SetBool("died", true);
        playerAnim.SetTrigger("DED");
        camAnim.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.position += new Vector3(0f, 0f, 10f);

        GetComponent<Move>().PlayerStop();

        endTime = Time.time + realtimeDuration;
        fadeOutTime = Time.time + realtimeFadeOutStart;
        isFadingOut = false;
        animRunning = true;

        DeathRingVFX.instance.PlayDeathAnimation();
    }



    public void Update()
    {
        if (animRunning)
        {

            // Start fade out
            if (Time.time >= fadeOutTime && !isFadingOut)
            {
                isFadingOut = true;
                ScreenFader.instance.FadeOut(0f, realtimeFadeOutDuration);
                AudioManager.Instance.MusicBranch.CrossfadeTo(MusicTrackName.HUB, 2f);
            }

            // perform loop
            if (Time.time >= endTime)
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
            AudioManager.Instance.SFXBranch.PlaySFXTrack("RespawnInOblivionSFX");
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
