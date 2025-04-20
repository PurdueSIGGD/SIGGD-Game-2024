using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private LeveledMusicTrack japan;
    [SerializeField] private MusicTrack seamstress;

    public enum MusicTrackName {
        //                  loopStart       loopEnd
        JAPAN, //           21.943          197.486
        SEAMSTRESS //       11.912          83.383    
    }

    private MusicTrackName currentTrackName;

    // Start is called before the first frame update
    void Start() { }
    
    // Update is called once per frame
    void Update() { }


    public IMusicTrack GetMusicTrack(MusicTrackName trackName) {
        switch (trackName) {
            case MusicTrackName.JAPAN:              return (IMusicTrack) japan;
            case MusicTrackName.SEAMSTRESS:         return (IMusicTrack) seamstress;
            default:                                return null;
        }
    }

    // Swaps the current track with NO crossfade
    public void PlayMusicTrack(MusicTrackName trackName) {
        GetCurrentMusicTrack().StopTrack();
        currentTrackName = trackName;
        GetCurrentMusicTrack().PlayTrack();
    }

    public IMusicTrack GetCurrentMusicTrack() {
        return GetMusicTrack(currentTrackName);
    }

    public MusicTrackName GetCurrentMusicTrackName() {
        return currentTrackName;
    }


    public void CrossfadeTo(MusicTrackName trackName, float fadeTime) {
        StartCoroutine(Crossfade(trackName, fadeTime));
    }

    // Fades into the given track over fadeTime seconds
    private IEnumerator Crossfade(MusicTrackName trackName, float fadeTime) {
        if (fadeTime <= 0) {
            PlayMusicTrack(trackName);
            yield return null;
        }

        int fadeSteps = 20;
        float stepTime = fadeTime / fadeSteps;
        IMusicTrack originalTrack = GetMusicTrack(currentTrackName);
        IMusicTrack newTrack = GetMusicTrack(trackName);
        
        // Need to be careful since these tracks might already be playing and have their own volume
        float originalTrackStartVolume = originalTrack.GetTrackVolume();
        float newTrackStartVolume = newTrack.GetTrackVolume();

        // The rate to change the tracks' volumes
        float originalTrackVolumeDelta = -originalTrackStartVolume / fadeSteps;
        float newTrackVolumeDelta = (1 - newTrackStartVolume) / fadeSteps;

        // Fade by adjusting volume over multiple steps
        newTrack.PlayTrack();  
        for (int i = 0; i <= fadeSteps; i++) {
            float originalTrackVolumeAdjustment = originalTrackVolumeDelta * i + originalTrackStartVolume;
            float newTrackVolumeAdjustment = newTrackVolumeDelta * i + newTrackStartVolume;


            originalTrack.SetTrackVolume(originalTrackVolumeAdjustment);
            newTrack.SetTrackVolume(newTrackVolumeAdjustment);            
            yield return new WaitForSeconds(stepTime);
        }
        originalTrack.StopTrack();
        currentTrackName = trackName;
    }
}
