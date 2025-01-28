using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 
/// 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialogueText; // Dialogue text

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(dialogueText.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
