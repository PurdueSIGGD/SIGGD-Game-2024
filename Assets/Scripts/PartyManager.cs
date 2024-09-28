using UnityEngine;
using UnityEngine.InputSystem;

// handles swapping of major ghosts and associated special actions
public class PartyManager : MonoBehaviour
{
    [SerializeField] Ghost[] ghosts;
    
    private int currentGhost;
    private GhostAction currentGhostAction;
    void Start()
    {
        currentGhost = 0;
        currentGhostAction = ghosts[currentGhost].specialAction;
    }

    void OnSpecial() {
        currentGhostAction.OnSpecial(this);
    }

    void OnSwitchSpecial() {
        currentGhost = (currentGhost + 1) % ghosts.Length;
        SwitchGhostAction(ghosts[currentGhost].specialAction);
    }
    void Update()
    {
        currentGhostAction.UpdateSpecial();
    }

    public void SwitchGhostAction(GhostAction newGhostAction)
    {
        if (currentGhostAction != null)
        {
            currentGhostAction.ExitSpecial();
        }

        currentGhostAction = newGhostAction;
        currentGhostAction.EnterSpecial();
    }
}
