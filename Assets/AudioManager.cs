using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // SOUNDTRACK
    [SerializeField] AudioTrack japanTheme;
    
    public enum MusicTrackName {
        JAPAN
    }

    // SFX
    [SerializeField] AudioTrack tempJelly;
    [SerializeField] AudioTrack tempInt;
    public enum SFXTrackName {
        TEMP_JELLY,
        TEMP_INT
    }

    public MusicTrackName currentTrack;
    // Start is called before the first frame update
    void Start()
    {
        japanTheme.Play();
        currentTrack = MusicTrackName.JAPAN;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            tempInt.Play();
        }   
    }
}
