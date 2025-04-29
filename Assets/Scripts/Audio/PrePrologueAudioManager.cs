using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePrologueAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Preprologue;
    [SerializeField] AudioSource BGMusic;
    [SerializeField] GameObject SceneManager;

    private float preprologueStartTime = 3f;
    public float BGMusicStartTimeAfterPreprologue = 8f;
    private float fadeOutSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Preprologue.PlayDelayed(preprologueStartTime);
        BGMusic.PlayDelayed(BGMusicStartTimeAfterPreprologue + preprologueStartTime);
        fadeOutSpeed = SceneManager.GetComponent<PrePrologueSceneManager>().fadeOutSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Phase out song and dialogue when space is held
    }
}
