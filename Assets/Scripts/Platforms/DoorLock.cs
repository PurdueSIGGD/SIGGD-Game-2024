using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Locks the door until player finishes dialogue with active ghosts
/// </summary>
public class DoorLock : MonoBehaviour
{
    [SerializeField] private ConversationName _lock;

    void Start()
    {
        
    }
    
    /// <summary>
    /// Set the correct conversation enum that needs to be finished
    /// to unlock the door
    /// </summary>
    /// <param name="_lock"></param>
    public void SetLock(ConversationName _lock)
    {
        this._lock = _lock;
    }

    /// <summary>
    /// Force unlock the door
    /// </summary>
    void UnlockDoor()
    {
        Door.activateDoor(true);
    }
}
