using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using static Door;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Vector2 waitRange = new(0.5f, 1.5f);
    [SerializeField] Slider progressBar;
    [SerializeField] TMP_Text readyText;
    
    private float origAudioVolume;
    private bool readyToProgress;
    private float waitTime;

    private float startFlashingTime;

    IEnumerator Start()
    {
        if (PlayerID.instance) PlayerID.instance.FreezePlayer();
        if (PlayerUIVisibility.instance) PlayerUIVisibility.instance.ShowPlayerUI();

        progressBar.value = 0f;
        waitTime = Random.Range(waitRange.x, waitRange.y);

        //yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.GetMusicVolume(out origAudioVolume);
        AudioManager.Instance.SetMusicVolume(-80f); // -80 is lowest possible volume
        AudioManager.Instance.MusicBranch.PlayMusicTrack(MusicTrackName.CYBERPUNK_LEVEL);
        AudioManager.Instance.SetEnergyLevel(0f);
        AudioManager.Instance.SetEnergyLevel(0.5f);
        AudioManager.Instance.SetEnergyLevel(1f);
        yield return new WaitForSeconds(waitTime);
        AllowPlayerContinue();
    }

    private void AllowPlayerContinue()
    {
        readyToProgress = true;
        startFlashingTime = Time.time;
        InputSystem.onAnyButtonPress.CallOnce(ctrl =>
        {
            OnDoorOpened?.Invoke();
            AudioManager.Instance.SetEnergyLevel(0f);
            AudioManager.Instance.SetMusicVolume(origAudioVolume);
        });
    }

    private void Update()
    {
        if (progressBar.value < 1f & waitTime != 0)
        {
            progressBar.value += Time.deltaTime / waitTime;
        }
        if (readyToProgress) 
        {
            float alpha = -Mathf.Cos(Time.time - startFlashingTime) + 0.5f;
            readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, alpha);
        }
    }
}
