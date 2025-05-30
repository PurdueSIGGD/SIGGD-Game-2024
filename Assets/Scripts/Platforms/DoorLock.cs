using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Locks the door until player finishes dialogue with active ghosts
/// </summary>
public class DoorLock : MonoBehaviour
{
    [SerializeField] private string _lock;

    void Start()
    {
        DialogueManager.onFinishDialogue += TryUnlock;
    }

    public void TryUnlock(string key)
    {
        if (key == _lock)
        {
            UnlockDoor();
        }
    }
    
    /// <summary>
    /// Set the correct conversation enum that needs to be finished
    /// to unlock the door
    /// </summary>
    public void SetLock(string _lock)
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
