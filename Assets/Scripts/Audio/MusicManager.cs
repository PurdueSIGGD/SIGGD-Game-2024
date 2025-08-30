using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //[SerializeField] private LeveledMusicTrack japan;
    //[SerializeField] private LeveledMusicTrack cyberpunk;
    //[SerializeField] private MusicTrack seamstress;
    [SerializeField] private LeveledMusicTrack cyberpunkLevel;
    [SerializeField] private LeveledMusicTrack feudalLevel;
    [SerializeField] private LeveledMusicTrack medivalLevel;
    [SerializeField] private MusicTrack hubLevel;
    [SerializeField] private MusicTrack irisTheme;
    [SerializeField] private MusicTrack noboruTheme;
    [SerializeField] private MusicTrack scatheTheme;
    [SerializeField] private MusicTrack policeChief;
    [SerializeField] private MusicTrack idol;
    [SerializeField] private MusicTrack seamstress;
    [SerializeField] private MusicTrack samurai;
    [SerializeField] private MusicTrack doctor;
    [SerializeField] private MusicTrack king;
    [SerializeField] private MusicTrack death;
    [SerializeField] private MusicTrack oldrionPhase13;
    [SerializeField] private MusicTrack oldrionphase4;

    private MusicTrackName currentTrackName;

    private void Awake()
    {
        currentTrackName = MusicTrackName.NULL;
    }
    
    // Update is called once per frame
    void Update() 
    {
        //GetCurrentMusicTrack().SetTrackVolume(0.07f);
    }


    public IMusicTrack GetMusicTrack(MusicTrackName trackName) {
        switch (trackName) {
            // case MusicTrackName.JAPAN:              return (IMusicTrack) japan;
            // case MusicTrackName.CYBERPUNK:          return (IMusicTrack) cyberpunk;
            
            case MusicTrackName.CYBERPUNK_LEVEL:       return (IMusicTrack) cyberpunkLevel;
            case MusicTrackName.HUB:                   return (IMusicTrack) hubLevel;
            case MusicTrackName.POLICE_CHIEF:          return (IMusicTrack) policeChief;
            case MusicTrackName.IDOL:                  return (IMusicTrack) idol;
            case MusicTrackName.MEDIVAL_LEVEL:         return (IMusicTrack) medivalLevel;
            case MusicTrackName.FEUDAL_LEVEL:          return (IMusicTrack) feudalLevel;
            case MusicTrackName.SEAMSTRESS:            return (IMusicTrack) seamstress;
            case MusicTrackName.IRIS_THEME:            return (IMusicTrack) irisTheme;
            case MusicTrackName.NOBORU_THEME:          return (IMusicTrack) noboruTheme;
            case MusicTrackName.SCATHE_THEME:          return (IMusicTrack) scatheTheme;
            case MusicTrackName.SAMURAI:               return (IMusicTrack) samurai;
            case MusicTrackName.DOCTOR:                return (IMusicTrack) doctor;
            case MusicTrackName.KING:                  return (IMusicTrack) king;
            case MusicTrackName.DEATH_THEME:           return (IMusicTrack) death;
            case MusicTrackName.OLDRION_FIRST:         return (IMusicTrack) oldrionPhase13;
            case MusicTrackName.OLDRION_FINAL:         return (IMusicTrack) oldrionphase4;
            default:                                   return null;
        }
    }

    // Swaps the current track with NO crossfade
    public void PlayMusicTrack(MusicTrackName trackName) {
        IMusicTrack cTrack = GetCurrentMusicTrack();
        if (cTrack != null) {
            GetCurrentMusicTrack().StopTrack();
        }
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
        if (trackName == MusicTrackName.NULL)
        {
            GetMusicTrack(currentTrackName).StopTrack();
        }
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
        float newTrackVolumeDelta = (originalTrackStartVolume) / fadeSteps;

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

public enum MusicTrackName {
    ////                  loopStart       loopEnd
    //JAPAN, //           21.943          197.486
    //CYBERPUNK, //       0.000           224.???
    //SEAMSTRESS //       11.912          83.383    

    //                    loopStart       loopEnd
    NULL, //              lol             lmao
    CYBERPUNK_LEVEL, //   0.000           224.000
    HUB, //               0.000           191.000
    POLICE_CHIEF, //      0.000           48.000
    IDOL, //              0.000           98.000
    MEDIVAL_LEVEL, //      what            bruh I dont know this
    FEUDAL_LEVEL, //      what            bruh I dont know this
    IRIS_THEME,
    NOBORU_THEME,
    SCATHE_THEME,
    SEAMSTRESS, //              0.000           98.000
    SAMURAI, //              0.000           98.000
    DOCTOR, //              0.000           98.000
    KING, //              0.000           98.000
    DEATH_THEME, //        0.000           51.000
    OLDRION_FIRST,
    OLDRION_FINAL
}