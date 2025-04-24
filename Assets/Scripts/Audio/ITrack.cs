
// Defines any type of track in the game SFX, music, or voice acting
// This is the master interface

using System;
using UnityEngine;

public interface ITrack {
    // Play the sound for  this track
    public void PlayTrack();
}