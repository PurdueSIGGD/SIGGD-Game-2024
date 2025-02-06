using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrack : MonoBehaviour
{
    // The sound file this script is attached to
    private AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play() {
        if (audio == null) { audio = GetComponent<AudioSource>(); }
        audio.PlayOneShot(audio.clip, 1.0f);
    }
}
