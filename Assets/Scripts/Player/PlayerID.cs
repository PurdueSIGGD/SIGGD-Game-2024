using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerID : MonoBehaviour
{
    public static PlayerID instance;

    // Private Variables
    private PlayerInput input;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    public void FreezePlayer()
    {
        input.enabled = false;
    }

    public void FreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Disable();
        input.actions.FindAction("Special").Disable();
    }

    public void UnfreezePlayer()
    {
        input.enabled = true;
    }

    public void UnfreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Enable();
        input.actions.FindAction("Special").Enable();
    }
}
