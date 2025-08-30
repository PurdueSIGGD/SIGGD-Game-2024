/// Party management script that tracks every active major ghost
/// Attaches to Player

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public static readonly int GHOST_LIMIT = 2;

    public static PartyManager instance;

    [SerializeField] int ghostLimit; // maximum number of ghosts player can wield at one time
    [SerializeField] private bool isStoryRoom;
    [SerializeField] GameObject[] allGhosts;

    private bool isSwappingEnabled = true;
    private bool isSwapping = false;
    private float swapRecoveryTimer = 0f;
    private float swapInputBuffer = 0f;
    private int swapInputBufferGhostIndex = 0;


    private Dictionary<string, GameObject> ghostsByName = new();
    private Dictionary<string, GhostIdentity> identitiesByName = new();

    // References to fields in SaveData, declared for convenience of a shorter name
    private List<string> ghostsInParty;
    public string selectedGhost = "Orion";
    private int selectedGhostIndex = -1;

    // simple gate for select last posessed ghost
    bool hasSelectedLastGhost = false;

    private void OnDoorOpen()
    {
        //SaveManager.instance.Save();
    }

    private void Awake()
    {
        //SaveManager.instance.Load();
        instance = this;

        /*foreach (GhostIdentity ghost in FindObjectsOfType<GhostIdentity>())
        {
            ghostsByName.Add(ghost.name, ghost);
        }*/

        foreach (GameObject ghost in allGhosts)
        {
            GhostIdentity g = ghost.GetComponent<GhostIdentity>();
            ghostsByName.Add(g.name, ghost);
            identitiesByName.Add(g.name, g);
        }

        if (!isStoryRoom)
        {
            ghostsInParty = SaveManager.data.ghostsInParty;
            selectedGhost = SaveManager.data.selectedGhost;
        }
        else
        {
            SaveManager.data.ghostsInParty = ghostsInParty = new List<string>();
            SaveManager.data.selectedGhost = selectedGhost = "Orion";
        }
    }

    private void Start()
    {
        Door.OnDoorOpened += OnDoorOpen;

        foreach (string ghost in ghostsInParty)
        {
            Debug.Log("Creating " + ghost);
            GhostIdentity identity = Instantiate(ghostsByName[ghost], Vector3.zero, new Quaternion()).GetComponent<GhostIdentity>();
            identity.TriggerEnterPartyBehavior();
            ghostsByName[ghost] = identity.gameObject;
            identitiesByName[ghost] = identity;
        }
    }

    private void Update()
    {
        if (swapInputBuffer > 0f) swapInputBuffer -= Time.deltaTime;
        if (swapRecoveryTimer > 0f && isSwapping) swapRecoveryTimer -= Time.deltaTime;
        if (swapRecoveryTimer <= 0f && isSwapping) isSwapping = false;
    }

    private void LateUpdate()
    {
        // ensures swap only after everything is loaded properly 
        if (!hasSelectedLastGhost)
        {
            SelectLastPosessedGhost();
            hasSelectedLastGhost = true;
        }
    }

    /// <summary>
    /// Adds ghost to end of player's ghost list
    /// </summary>
    /// <param ghostName="ghost"></param>
    public bool TryAddGhostToParty(GhostIdentity ghost)
    {
        if (ghostsInParty.Count < GHOST_LIMIT && !ghostsInParty.Contains(ghost.name))
        {
            ghostsInParty.Add(ghost.name);
            identitiesByName[ghost.name] = ghost;
            ghostsByName[ghost.name] = ghost.gameObject;
            ghost.TriggerEnterPartyBehavior();

            // try removing indicator, if any
            GhostInteract ghostInteract = ghost.GetComponent<GhostInteract>();
            if (ghostInteract) ghostInteract.DisableIndicator();

            if (isStoryRoom)
            {
                SaveManager.data.ghostsInParty = ghostsInParty;
            }

            ghost.gameObject.GetComponent<GhostUIDriver>().UpdatePartyStatus();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Called whenever the hotbar action is triggered by player input,
    /// ignoring `0` value inputs
    /// </summary>
    public void OnHotbar(InputValue value)
    {
        //Debug.Log("HOTBAR: " + (int)value.Get<float>());
        int keyValue = (int)value.Get<float>();
        if (keyValue != 0)
        {
            // hotkey #2 is 0th ghost index in list
            ChangePosessingGhost(keyValue - 2);
        }
    }

    public void OnScrollWheel(InputValue value)
    {
        //Debug.Log("SCROLL WHEEL INPUT: " + value.Get<float>());
        int scrollWheelValue = (int)value.Get<float>();
        int newIndex = selectedGhostIndex;
        /*
        if (scrollWheelValue == 0)
        {
            return;
        }
        else if (scrollWheelValue < 0)
        {
            newIndex = selectedGhostIndex - 1;
            if (newIndex < -1) newIndex = ghostsInParty.Count - 1;
        }
        else if (scrollWheelValue > 0)
        {
            newIndex = selectedGhostIndex + 1;
            if (newIndex > ghostsInParty.Count - 1) newIndex = -1;
        }
        ChangePosessingGhost(newIndex);
        */

        // Invalid swap input
        if (scrollWheelValue == 0)
        {
            return;
        }

        // Swap to Orion
        else if (scrollWheelValue < 0)
        {
            newIndex = -1;
        }

        // Swap ghosts
        else if (scrollWheelValue > 0)
        {
            // Swap to ghost 2
            if (selectedGhostIndex == 0)
            {
                newIndex = 1;
            }

            // Swap to ghost 1
            else if (selectedGhostIndex == 1 || selectedGhostIndex == -1)
            {
                newIndex = 0;
            }

            // Enforce party size
            if (newIndex >= ghostsInParty.Count)
            {
                newIndex = ghostsInParty.Count - 1;
            }
        }

        // Swap
        ChangePosessingGhost(newIndex);
    }

    /// <summary>
    /// If present in the party, autoselects the last possessed ghost as stated in SaveManager.
    /// </summary>
    public void SelectLastPosessedGhost()
    {
        const int INVALID_INDEX = -9999;
        string lastGhost = SaveManager.data.selectedGhost;
        print("LAST GHOST: " + lastGhost);
        int lastGhostIndex = INVALID_INDEX;
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            print("GHOST NAMES: " + ghostsInParty[i]);
            if (ghostsInParty[i].Equals(lastGhost))
            {
                lastGhostIndex = i;
                break;
            }
        }
        if (lastGhostIndex == INVALID_INDEX)
        {
            return;
        }
        SwitchGhostToIndex(lastGhostIndex);
    }

    /// <summary>
    /// Switching ghost functionality
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Name of newly selected ghost, or an error code: "invalid" - bad index arg; "current" - index is current ghost</returns>
    public string SwitchGhostToIndex(int index)
    {
        // handle bad input
        if (index >= ghostsInParty.Count) return "invalid";

        // Don't swap if ghost is already possessing
        if (selectedGhostIndex == index) return "current";

        // deselect all ghosts in the list
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            ghostsByName[ghostsInParty[i]].GetComponent<GhostIdentity>()?.TriggerDeSelectedBehavior();
        }
        selectedGhostIndex = index;

        // do not possess if player selected base kit
        if (index == -1)
        {
            selectedGhost = "Orion";
            return selectedGhost;
        }

        ghostsByName[ghostsInParty[index]].GetComponent<GhostIdentity>().TriggerSelectedBehavior();
        selectedGhost = ghostsInParty[index];

        return selectedGhost;
    }

    /// <summary>
    /// Switches the currently posessing ghost based on hotkey input (1,2,3, etc.)
    /// </summary>
    /// <param name="inputNum">The index to select from the list</param>
    public void ChangePosessingGhost(int index)
    {
        if (isSwapping) return;

        //GetComponent<Animator>().SetTrigger("toIdle");
        GetComponent<Move>().PlayerGo();
        // Don't swap if disabled, start swap input buffer
        if (!isSwappingEnabled)
        {
            swapInputBuffer = 0.3f;
            swapInputBufferGhostIndex = index;
            return;
        }

        swapRecoveryTimer = 0.3f;
        isSwapping = true;

        string result = SwitchGhostToIndex(index);
        if (result == "invalid" || result == "current")
        {
            return;
        }
        AudioManager.Instance.VABranch.PlayVATrack(selectedGhost + " On Swap");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("GhostSwap");
        SaveManager.data.selectedGhost = selectedGhost;
    }

    /// <summary>
    /// Removes from player's major ghost list based off reference
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public bool RemoveGhostFromParty(GhostIdentity ghost)
    {
        bool success = ghostsInParty.Remove(ghost.name);

        // re-add dialogue interaction indiactor, if any
        if (ghost && success)
        {
            GhostInteract ghostInteract = ghost.GetComponent<GhostInteract>();
            ghostInteract.EnableIndiactor(); 
            if (isStoryRoom) ghostInteract.ReturnGhostToOrigPos();
        }

        ghost.gameObject.GetComponent<GhostUIDriver>().UpdatePartyStatus();

        Debug.Log("Saving Ghosts");
        if (isStoryRoom)
        {
            SaveManager.data.ghostsInParty = ghostsInParty;
        }

        return success;
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostPartyList()
    {
        return ghostsInParty.Select(ghostName => identitiesByName[ghostName]).ToList();
    }

    public GhostIdentity GetSelectedGhost()
    {
        return identitiesByName.GetValueOrDefault(selectedGhost);
    }

    public bool IsGhostInParty(GhostIdentity ghost)
    {
        return ghostsInParty.Contains(ghost.name.Replace("(Clone)", ""));
    }

    public bool IsGhostInParty(string ghostName)
    {
        return ghostsInParty.Contains(ghostName.Replace("(Clone)", ""));
    }

    /// <summary>
    /// Enable or disable the ability to swap the active ghost. If a swap is attempted while disabled, the input is buffered for 0.3 seconds.
    /// </summary>
    /// <param name="enabled">If true, ghost swapping will be enabled. If false, ghost swapping will be disabled.</param>
    public void SetSwappingEnabled(bool enabled)
    {
        if ((!isSwappingEnabled && enabled) && swapInputBuffer > 0f)
        {
            isSwappingEnabled = true;
            swapInputBuffer = 0f;
            ChangePosessingGhost(swapInputBufferGhostIndex);
            return;
        }
        isSwappingEnabled = enabled;
    }

    /// <summary>
    /// Getter for identities by name
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GhostIdentity> GetIdentitiesByName()
    {
        return identitiesByName;
    }
}
