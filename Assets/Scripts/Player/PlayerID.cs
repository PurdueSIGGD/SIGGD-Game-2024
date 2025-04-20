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
        Debug.Log("AAA: Freeze");
    }

    public void FreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Disable();
        input.actions.FindAction("Special").Disable();
        Debug.Log("AAA: Freeze Mouse");
    }

    public void UnfreezePlayer()
    {
        input.enabled = true;
        Debug.Log("AAA: Unfreeze");
    }

    public void UnfreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Enable();
        input.actions.FindAction("Special").Enable();
        Debug.Log("AAA: Unfreeze Mouse");
    }
}
