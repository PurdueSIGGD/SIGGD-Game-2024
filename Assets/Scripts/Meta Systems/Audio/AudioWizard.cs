#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioWizard : ScriptableWizard
{
    [SerializeField] string audioName;
    [SerializeField] AudioType audioType;
    [SerializeField] TrackType trackType;
    [SerializeField] AudioClip[] clips;

    enum AudioType
    {
        Music,
        VA,
        SFX
    }

    enum TrackType
    {
        OneShot,
        Looping,
        SoundBank,
        Leveled
    }

    [MenuItem("SIGGD/Create New Audio")]
    static void CreateWizard()
    {
        DisplayWizard<AudioWizard>("Add new Audio");
    }

    private void OnWizardCreate()
    {
        // Validate inputs
        if (!ValidInputs()) { return; }

        // load the AudioManager prefab
        GameObject audioManager = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Meta Systems/Audio Manager.prefab");

        Transform parent = audioManager.transform.Find("Dialogue");

        GameObject newDialogue = new GameObject(audioName);
        newDialogue.transform.parent = parent;

        ConversationAudioHolder conversationAudioHolder = newDialogue.AddComponent<ConversationAudioHolder>();
        conversationAudioHolder.tracks = new List<OneShotVATrack>();

        foreach (AudioClip clip in clips)
        {
            AudioSource source = newDialogue.AddComponent<AudioSource>();
            OneShotVATrack oneShotVATrack = newDialogue.AddComponent<OneShotVATrack>();

            source.clip = clip;
            source.playOnAwake = false;

            oneShotVATrack.track = source;
            conversationAudioHolder.tracks.Add(oneShotVATrack);
        }


        // save the AudioManager prefab
        PrefabUtility.SaveAsPrefabAsset(audioManager, "Assets/Prefabs/Meta Systems/Audio Manager.prefab");
    }

    private bool ValidInputs()
    {
        if (audioName == null)
        {
            Debug.LogWarning("Name field must not be empty when adding audio");
            return false;
        }
        if (clips.Length == 0)
        {
            Debug.LogWarning("You must add at least one audio clip");
            return false;
        }
        return true;
    }
}

#endif
