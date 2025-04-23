/// Party management script that tracks every active major ghost
/// Attaches to Player

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public static readonly int GHOST_LIMIT = 2;

    public static PartyManager instance;

    private List<GhostIdentity> ghostsInParty = new List<GhostIdentity>(); // list of each ghost in party
    private GhostIdentity selectedGhost = null;

    private bool isSwappingEnabled = true;
    private float swapInputBuffer = 0f;
    private int swapInputBufferGhostIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (swapInputBuffer > 0f) swapInputBuffer -= Time.deltaTime;
    }

    /// <summary>
    /// Adds ghost to end of player's ghost list
    /// </summary>
    /// <param ghostName="ghost"></param>
    public bool TryAddGhostToParty(GhostIdentity ghost)
    {
        if (ghostsInParty.Count < GHOST_LIMIT && !ghostsInParty.Contains(ghost))
        {
            ghostsInParty.Add(ghost);
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

    /// <summary>
    /// Switches the currently posessing ghost based on hotkey input (1,2,3, etc.)
    /// </summary>
    /// <param name="inputNum">The index to select from the list(value is either -1(player kit), 0, or 1)</param>
    public void ChangePosessingGhost(int index)
    {
        GetComponent<Animator>().SetTrigger("toIdle");
        GetComponent<Move>().PlayerGo();
        // Don't swap if disabled, start swap input buffer
        if (!isSwappingEnabled)
        {
            swapInputBuffer = 0.3f;
            swapInputBufferGhostIndex = index;
            return;
        }

        // handle bad input
        if (index >= ghostsInParty.Count) return;

        // Don't swap if ghost is already possessing
        if (index >= 0 && ghostsInParty[index].Equals(selectedGhost)) return;

        // deselect all ghosts in the list
        for (int i = 0; i < ghostsInParty.Count; i++)
        {
            ghostsInParty[i].TriggerDeSelectedBehavior();
        }

        // do not possess if player selected base kit
        if (index == -1)
        {
            selectedGhost = null;
            return;
        }

        ghostsInParty[index].TriggerSelectedBehavior();
        selectedGhost = ghostsInParty[index];
    }

    /// <summary>
    /// Removes from player's major ghost list based off reference
    /// </summary>
    /// <param ghostName="ghostIndex"></param>
    public bool RemoveGhostFromParty(GhostIdentity ghost)
    {
        return ghostsInParty.Remove(ghost);
    }

    /// <summary>
    /// Allow external scripts to access player's major ghosts
    /// </summary>
    /// <returns></returns>
    public List<GhostIdentity> GetGhostPartyList()
    {
        return ghostsInParty;
    }

    public GhostIdentity GetSelectedGhost()
    {
        return selectedGhost;
    }

    public bool IsGhostInParty(GhostIdentity ghost)
    {
        return ghostsInParty.Contains(ghost);
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
}
