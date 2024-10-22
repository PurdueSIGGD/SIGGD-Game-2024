using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
/// <summary>
/// Handles swapping of major ghosts and associated special actions
/// </summary>
public class PartyManagerOld : MonoBehaviour
{
    [SerializeField] Ghost[] ghosts; // Holds major ghosts currently in party
    
    public TextMeshProUGUI specialText;
    private int currentGhost;
    private GhostAction currentGhostAction; 
    void Start()
    {
        currentGhost = 0;
        currentGhostAction = ghosts[currentGhost].specialAction;
        specialText.text = ghosts[currentGhost].name;
    }

    /// <summary>
    /// Called when the player uses the special action
    /// </summary>
    void OnSpecial() {
        currentGhostAction.OnSpecial(this);
    }

    /// <summary>
    /// Called when the player switches the major ghost
    /// </summary>
    void OnSwitchSpecial() {
        currentGhost = (currentGhost + 1) % ghosts.Length;
        if (currentGhostAction != null)
        {
            currentGhostAction.ExitSpecial();
        }

        currentGhostAction = ghosts[currentGhost].specialAction;
        currentGhostAction.EnterSpecial();
        specialText.text = ghosts[currentGhost].name;
    }
    void Update()
    {
        currentGhostAction.UpdateSpecial();
    }
}
