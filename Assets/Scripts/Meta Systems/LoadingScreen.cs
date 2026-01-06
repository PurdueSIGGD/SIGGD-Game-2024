using System.Collections;
using UnityEngine;
using static Door;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Vector2 waitRange = new(0.5f, 1.5f);

    IEnumerator Start()
    {
        if (PlayerID.instance) PlayerID.instance.FreezePlayer();

        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.GetMusicVolume(out float originalVolume);
        AudioManager.Instance.SetMusicVolume(-80f); // -80 is lowest possible volume
        AudioManager.Instance.MusicBranch.PlayMusicTrack(MusicTrackName.CYBERPUNK_LEVEL);
        AudioManager.Instance.SetEnergyLevel(1f);
        AudioManager.Instance.SetEnergyLevel(0.5f); 
        AudioManager.Instance.SetEnergyLevel(0f);
        yield return new WaitForSeconds(Random.Range(waitRange.x, waitRange.y));
        OnDoorOpened?.Invoke();
        AudioManager.Instance.SetMusicVolume(originalVolume);
    }
}
