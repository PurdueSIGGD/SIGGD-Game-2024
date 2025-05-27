#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioWizard : ScriptableWizard
{
    [SerializeField] string audioName;
    [SerializeField] AudioType audioType;
    [SerializeField] TrackType trackType;
    [SerializeField] Mixer mixerType;
    [SerializeField] string sourcePath = "";
    [SerializeField] List<AudioClip> clips;
    [SerializeField] bool enableVoiceCulling;

    private AudioLookUpTable lookUpTable;
    private AudioMixer audioMixer;

    // used for updating wizard ui
    private AudioType oldAudioType = AudioType.VA;
    private string oldPath = "";

    enum AudioType
    {
        VA,
        SFX,
        Conversation
    }

    enum TrackType
    {
        SoundBank,
        OneShot,
        Looping,
        Leveled
    }

    enum Mixer
    {
        VA,
        AmbientSFX,
        BIGSFX,
        DefaultSFX,
        Master,
        Music,
        PrioritySFX,
        SFX
    }

    private void OnEnable()
    {
        audioMixer = AssetDatabase.LoadAssetAtPath("Assets/Audio/Mixer.mixer", typeof(AudioMixer)) as AudioMixer;
    }

    [MenuItem("SIGGD/Create New Audio")]
    static void CreateWizard()
    {
        DisplayWizard<AudioWizard>("Add new Audio", "Create", "Refresh");
    }

    private void OnWizardCreate()
    {
        // Validate inputs
        if (!ValidInputs()) { return; }

        // load the AudioManager prefab
        GameObject audioManager = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Meta Systems/Audio Manager.prefab");
        lookUpTable = audioManager.GetComponent<AudioLookUpTable>();

        Transform parent = FindParent(audioManager);
        GameObject audioComp = new(audioName);
        audioComp.transform.parent = parent;

        // ok below should be some brilliant code that just generically fill up the audio component
        // BUT each audio and track type is like created so diffferently
        // that it doesn't feel worth it at this point.
        // If you are reading this (why?? hopefully not because my code is throwing a trillion errors)
        // and feel u can fix it, by all means give it a try, thanks xoxo

        // ok disclaimers aside, I'm gonna start writing this monstrosity now god help me

        // iterate each child object, if name already eixsts, add to the existing
        // child object instead of creating a new one
        bool addToExisting = false;
        foreach (Transform child in audioManager.GetComponentsInChildren<Transform>())
        {
            if (child.name.Equals(audioName))
            {
                addToExisting = true;
                break;
            }
        }


        switch (audioType)
        {
            case AudioType.VA:
                CreateVA(audioComp, addToExisting);
                if (ReloadKeys(audioManager)) break;
                else return;
            case AudioType.SFX:
                CreateSFX(audioComp, addToExisting);
                if (ReloadKeys(audioManager)) break;
                else return;
            case AudioType.Conversation:
                CreateConversation(audioComp, addToExisting);
                if (ReloadKeys(audioManager)) break;
                else return;
            default:
                Debug.LogWarning("code fell through while creating audio holder, aborting save");
                return;
        }


        // save the AudioManager prefab
        EditorUtility.SetDirty(audioManager);
        PrefabUtility.SaveAsPrefabAsset(audioManager, "Assets/Prefabs/Meta Systems/Audio Manager.prefab");
    }

    private void OnWizardOtherButton()
    {
        GameObject audioManager = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Meta Systems/Audio Manager.prefab");
        ReloadKeys(audioManager);
        PrefabUtility.SaveAsPrefabAsset(audioManager, "Assets/Prefabs/Meta Systems/Audio Manager.prefab");
    }

    private void OnWizardUpdate()
    {
        // if the audio type has changed, make corresponding changes to default set up
        if (oldAudioType != audioType)
        {
            oldAudioType = audioType;
            if (audioType == AudioType.VA)
            {
                mixerType = Mixer.VA;
                trackType = TrackType.SoundBank;
            }
            if (audioType == AudioType.Conversation)
            {
                mixerType = Mixer.VA;
                trackType = TrackType.OneShot;
            }
        }

        // if the source path has changed, attempt to read it
        if (!sourcePath.Equals(oldPath))
        {
            oldPath = sourcePath;
            String[] loadedGUID = AssetDatabase.FindAssets("", new[] { sourcePath });

            if (loadedGUID.Length > 0 && clips == null)
            {
                clips = new List<AudioClip>();
            }

            foreach (string guid in loadedGUID)
            {
                String path = AssetDatabase.GUIDToAssetPath(guid);
                AudioClip clip = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;
                clips.Add(clip);
            }
        }
    }




    // =================
    // helper functions
    // =================

    private bool ValidInputs()
    {
        if (audioName == null)
        {
            Debug.LogWarning("Name field must not be empty when adding audio");
            return false;
        }
        if (clips.Count == 0)
        {
            Debug.LogWarning("You must add at least one audio clip");
            return false;
        }
        return true;
    }

    private Transform FindParent(GameObject audioManager)
    {
        switch (audioType)
        {
            case AudioType.VA:
                return audioManager.transform.Find("VA").Find("Voice Line");
            case AudioType.SFX:
                return audioManager.transform.Find("SFX");
            case AudioType.Conversation:
                return audioManager.transform.Find("VA").Find("Conversation");
            default:
                Debug.LogWarning("Cannot find component under Audio Manager Prefab");
                return null;
        }
    }

    private void CreateVA(GameObject audioComp, bool addToExisting)
    {
        switch (trackType)
        {
            case TrackType.SoundBank:
                {
                    SoundBankVATrack soundBankVATrack;
                    if (addToExisting) soundBankVATrack = audioComp.GetComponent<SoundBankVATrack>();
                    else
                    {
                        soundBankVATrack = audioComp.AddComponent<SoundBankVATrack>();
                        soundBankVATrack.sounds = new List<UnityEngine.Object>();
                        soundBankVATrack.soundWeights = new List<float>();
                        soundBankVATrack.recencyBlacklistSize = 1;
                    }

                    foreach (AudioClip clip in clips)
                    {
                        AudioSource source = audioComp.AddComponent<AudioSource>();
                        source.clip = clip;
                        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
                        source.playOnAwake = false;

                        OneShotVATrack oneShotVATrack = audioComp.AddComponent<OneShotVATrack>();
                        oneShotVATrack.track = source;
                        oneShotVATrack.voiceCullingOverride = enableVoiceCulling;

                        soundBankVATrack.sounds.Add(oneShotVATrack);
                        soundBankVATrack.soundWeights.Add(1);
                    }
                    break;
                }
            case TrackType.OneShot:
                {
                    AudioSource source;
                    if (addToExisting) source = audioComp.GetComponent<AudioSource>();
                    else source = audioComp.AddComponent<AudioSource>();
                    source.clip = clips[0];
                    source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
                    source.playOnAwake = false;

                    OneShotVATrack oneShotVATrack;
                    if (addToExisting) oneShotVATrack = audioComp.GetComponent<OneShotVATrack>();
                    else oneShotVATrack = audioComp.AddComponent<OneShotVATrack>();
                    oneShotVATrack.track = source;
                    oneShotVATrack.voiceCullingOverride = enableVoiceCulling;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Unable to create Voice Line, only supports creating Track Type: One Shot and Soundbank");
                    break;
                }
        }

    }

    private void CreateSFX(GameObject audioComp, bool addToExisting)
    {
        switch (trackType)
        {
            case TrackType.OneShot: 
                {
                    AudioSource source;
                    if (addToExisting) source = audioComp.GetComponent<AudioSource>();
                    else source = audioComp.AddComponent<AudioSource>();
                    source.clip = clips[0];
                    source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
                    source.playOnAwake = false;

                    OneShotSFXTrack oneShotSFXTrack;
                    if (addToExisting) oneShotSFXTrack = audioComp.GetComponent<OneShotSFXTrack>();
                    else oneShotSFXTrack = audioComp.AddComponent<OneShotSFXTrack>();
                    oneShotSFXTrack.track = source;
                    break;
                }
            case TrackType.Looping:
                {
                    AudioSource source1;
                    AudioSource source2;
                    if (addToExisting) source1 = source2 = audioComp.GetComponent<AudioSource>();
                    else source1 = source2 = audioComp.AddComponent<AudioSource>();
                    source1.clip = source2.clip = clips[0];
                    source1.outputAudioMixerGroup = source2.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
                    source1.playOnAwake = source2.playOnAwake = false;

                    LoopingSFXTrack loopingSFXTrack;
                    if (addToExisting) loopingSFXTrack = audioComp.GetComponent<LoopingSFXTrack>();
                    else loopingSFXTrack = audioComp.AddComponent<LoopingSFXTrack>();
                    loopingSFXTrack.tracks = new AudioSource[2];
                    loopingSFXTrack.tracks[0] = source1;
                    loopingSFXTrack.tracks[1] = source2;
                    break;
                }
            case TrackType.SoundBank:
                {
                    SoundBankSFXTrack soundBankSFXTrack;
                    if (addToExisting) soundBankSFXTrack = audioComp.GetComponent<SoundBankSFXTrack>();
                    else 
                    {
                        soundBankSFXTrack = audioComp.AddComponent<SoundBankSFXTrack>();
                        soundBankSFXTrack.sounds = new List<UnityEngine.Object>();
                        soundBankSFXTrack.soundWeights = new List<float>();
                    }

                    foreach (AudioClip clip in clips)
                    {
                        AudioSource source = audioComp.AddComponent<AudioSource>();
                        source.clip = clip;
                        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
                        source.playOnAwake = false;

                        OneShotSFXTrack oneShotSFXTrack = audioComp.AddComponent<OneShotSFXTrack>();
                        oneShotSFXTrack.track = source;

                        soundBankSFXTrack.sounds.Add(oneShotSFXTrack);
                        soundBankSFXTrack.soundWeights.Add(1);
                    }
                    break;
                }
            default:
                {
                    Debug.LogWarning("Unable to create SFX, only supports creating Track Type: One Shot, Looping, and Soundbank");
                    break;
                }
        }
    }

    private void CreateConversation(GameObject audioComp, bool addToExisting)
    {
        ConversationAudioHolder conversationAudioHolder;
        if (addToExisting) conversationAudioHolder = audioComp.GetComponent<ConversationAudioHolder>();
        else
        {
            conversationAudioHolder = audioComp.AddComponent<ConversationAudioHolder>();
            conversationAudioHolder.tracks = new List<OneShotVATrack>();
        }

        foreach (AudioClip clip in clips)
        {
            AudioSource source = audioComp.AddComponent<AudioSource>();
            OneShotVATrack oneShotVATrack = audioComp.AddComponent<OneShotVATrack>();

            source.clip = clip;
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(GetSelectedMixer())[0];
            source.playOnAwake = false;

            oneShotVATrack.track = source;
            conversationAudioHolder.tracks.Add(oneShotVATrack);
        }
    }

    private string GetSelectedMixer()
    {
        switch (mixerType)
        {
            case Mixer.BIGSFX: return "BIGSFX";
            case Mixer.AmbientSFX: return "AmbientSFX";
            case Mixer.SFX: return "SFX";
            case Mixer.PrioritySFX: return "PrioritySFX";
            case Mixer.Music: return "Music";
            case Mixer.DefaultSFX: return "DefaultSFX";
            case Mixer.Master: return "Master";
            case Mixer.VA: return "VA";
            default:
                Debug.LogWarning("Cannot find selected mixer");
                return "";
        }
    }

    private bool ReloadKeys(GameObject audioManager)
    {
        lookUpTable = audioManager.GetComponent<AudioLookUpTable>();
        lookUpTable.SFXTracks = new();
        lookUpTable.SFXLoops = new();
        lookUpTable.SFXBanks = new();
        lookUpTable.conversationTracks = new();
        lookUpTable.VABanks = new();
        lookUpTable.VATracks = new();

        foreach (Transform child in audioManager.GetComponentsInChildren<Transform>())
        {   
            OneShotSFXTrack oneShotSFXTrack = child.GetComponent<OneShotSFXTrack>();
            LoopingSFXTrack loopingSFXTrack = child.GetComponent<LoopingSFXTrack>();
            SoundBankSFXTrack soundBankSFXTrack = child.GetComponent<SoundBankSFXTrack>();

            if (soundBankSFXTrack)
            {
                SFXBank sfxBank = new(child.name, soundBankSFXTrack);
                lookUpTable.SFXBanks.Add(sfxBank);
                continue;
            }
            if (oneShotSFXTrack)
            {
                SFXTrack sfxTrack = new(child.name, oneShotSFXTrack); 
                lookUpTable.SFXTracks.Add(sfxTrack);
                continue;
            }
            if (loopingSFXTrack)
            {
                SFXLoop sfxLoop = new(child.name, loopingSFXTrack);
                lookUpTable.SFXLoops.Add(sfxLoop);
                continue;
            }

            ConversationAudioHolder conversationAudioHolder = child.GetComponent<ConversationAudioHolder>();
            if (conversationAudioHolder)
            {
                ConversationTrack conversation = new(child.name, conversationAudioHolder);
                lookUpTable.conversationTracks.Add(conversation);
                continue;
            }

            SoundBankVATrack soundBankVATrack = child.GetComponent<SoundBankVATrack>();
            OneShotVATrack oneShotVATrack = child.GetComponent<OneShotVATrack>();

            if (soundBankVATrack)
            {
                VABank bank = new(child.name, soundBankVATrack);
                lookUpTable.VABanks.Add(bank);
                continue;
            }
            if (oneShotVATrack)
            {
                VATrack track = new(child.name, oneShotVATrack);
                lookUpTable.VATracks.Add(track);
                continue;
            }
        }
        return true;
    }
}

#endif
