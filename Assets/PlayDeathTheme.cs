using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDeathTheme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.MusicBranch.PlayMusicTrack(MusicTrackName.DEATH_THEME);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
