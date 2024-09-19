using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Consult with the META SYSTEMS team for any doubt about how this works ;-;
 */

/* Enum used to classify all types of sound effects related to a certain action or reaction */
public enum SoundManager_Enum {
    PLAYER_ATTACK,
    PLAYER_HURT,
    ENEMY_ATTACK,
    ENEMY_HURT
}

/* Struct used for aesthetic purposes (make it look good and easy to use in the inspector) */
[Serializable]
public struct SoundList_t
{
    public AudioClip[] get_sounds { get => sounds; }
    [HideInInspector] public string name; // used to match with the corresponding enum
    [SerializeField] private AudioClip[] sounds; // array that stores the literal sounds
    [SerializeField] public bool enabled; // bool to make some sounds get disabled (performance once the game scales)
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager_Script : MonoBehaviour
{
    [SerializeField] private SoundList_t[] sound_list;
    private static SoundManager_Script instance;
    private AudioSource audio_source;

    /* Made it a singleton, hopefully doesn't go to shit ._. */
    private void Awake() { instance = this; }

    private void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    /* External scripts call this function to get their sounds at a specific moment */
    public static void PlaySound(SoundManager_Enum sound_enum, float volume = 1f)
    {
        SoundList_t sound_category = instance.sound_list[(int) sound_enum];

        /* This checks if the sound is enabled (made it for performance issues that may arise through scale) */
        if (!sound_category.enabled)
        {
            Debug.Log($"Sound category {sound_enum} is disabled.");
            return;
        }

        /* Random audio selection for multiple sounds that are of the same type (e.g. hurt1, hurt2, hurt3, etc) */
        AudioClip[] audio_clips = instance.sound_list[(int) sound_enum].get_sounds;
        AudioClip random_clip = audio_clips[UnityEngine.Random.Range(0, audio_clips.Length)];
        instance.audio_source.PlayOneShot(random_clip, volume);
    }
    
    /* Just sets up the initial way the sound manager looks in the editor */
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundManager_Enum));
        Array.Resize(ref sound_list, names.Length);
        for (int i = 0; i < sound_list.Length; i++) {
            sound_list[i].name = names[i];
            
            if (sound_list[i].enabled == false) {
                sound_list[i].enabled = true;
            }
        }
    }
#endif
}
