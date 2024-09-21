using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SecondaryAudioClip
{
    public string id;
    public AudioClip clip;
    public bool loop;
    public bool is_enabled;
    public float start_time;
    public float end_time;
    [Range(0, 1)]public float relative_volume;
}

[System.Serializable]
public struct Suite
{
    public string id; // Use capital letters with underscores (e.g., TOKYO_STREETS)
    public AudioClip main_clip;
    public bool loop_main;
    public float loop_start_time;
    public SecondaryAudioClip[] secondary_clips;
}


public class MusicManager_Script : MonoBehaviour
{
    public Suite[] suite;
}
