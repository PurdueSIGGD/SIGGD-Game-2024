using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EvaSongTracker : MonoBehaviour
{

    [SerializeField] string dialogue;
    [SerializeField] DialogueTriggerBox triggerBox;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.onFinishDialogue += TrySing;
    }

    public void TrySing(string key)
    {
        if (key == dialogue)
        {
            AudioManager.Instance.GetComponentInChildren<MusicManager>().CrossfadeTo(MusicTrackName.EVA_SONG, 0.5f);
        }
        StartCoroutine(ActivateCoroutine(94));
    }

    private IEnumerator ActivateCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        triggerBox.active = true;
        AudioManager.Instance.GetComponentInChildren<MusicManager>().CrossfadeTo(MusicTrackName.IDOL, 2f);
    }
}
