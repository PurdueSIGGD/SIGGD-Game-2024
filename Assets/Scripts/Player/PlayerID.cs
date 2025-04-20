using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerID : MonoBehaviour
{
    // ==============================
    //       Serialized Fields
    // ==============================


    // ==============================
    //        Other Variables
    // ==============================

    public static PlayerID instance;

    // Private Variables
    private PlayerInput input;

    // ==============================
    //        Unity Functions
    // ==============================

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    // ==============================
    //       Private Functions
    // ==============================


    // ==============================
    //        Public Functions
    // ==============================

    /// <summary>
    /// Freezes the player, making player unresponsive to input
    /// </summary>
    public void FreezePlayer()
    {
        input.enabled = false;
        Debug.Log("AAA: Freeze");
    }

    /// <summary>
    /// Freezes the player's mouse, making player unresponsive to mouse input
    /// </summary>
    public void FreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Disable();
        input.actions.FindAction("Special").Disable();
        Debug.Log("AAA: Freeze Mouse");
    }

    /// <summary>
    /// Unfreezes the player, making player responsive to input again
    /// </summary>
    public void UnfreezePlayer()
    {
        input.enabled = true;
        Debug.Log("AAA: Unfreeze");
    }

    /// <summary>
    /// Unfreezes the player, making player responsive to mouse input again
    /// </summary>
    public void UnfreezePlayerMouse()
    {
        input.actions.FindAction("Attack").Enable();
        input.actions.FindAction("Special").Enable();
        Debug.Log("AAA: Unfreeze Mouse");
    }
}
