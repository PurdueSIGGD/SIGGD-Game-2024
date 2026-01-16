using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
#endif

public class ConvoTester : MonoBehaviour
{
    [SerializeField] string convoSourceFolder;
    [SerializeField] Transform buttonHolder;
    [SerializeField] GameObject convoButton;
    [SerializeField] ConvoSO[] allConvo;


#if UNITY_EDITOR
    [ContextMenu("Populate Convos From Folder")]
    private void PopulateConvos()
    {
        // Ensure the folder path is valid or default to Assets
        if (string.IsNullOrEmpty(convoSourceFolder)) return;
        if (convoSourceFolder.EndsWith("/")) convoSourceFolder = convoSourceFolder.Substring(0, convoSourceFolder.Length - 1);

        if (!AssetDatabase.IsValidFolder(convoSourceFolder))
        {
            Debug.LogError($"Folder not found: {convoSourceFolder}");
            return;
        }

        // Find all assets of type ConvoSO in the specified folder (recursive)
        string[] guids = AssetDatabase.FindAssets("t:ConvoSO", new[] { convoSourceFolder });
        List<ConvoSO> foundConvos = new List<ConvoSO>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ConvoSO convo = AssetDatabase.LoadAssetAtPath<ConvoSO>(path);
            if (convo != null)
            {
                foundConvos.Add(convo);
            }
        }

        allConvo = foundConvos.ToArray();
        
        EditorUtility.SetDirty(this);
        Debug.Log($"Successfully populated {allConvo.Length} ConvoSOs from '{convoSourceFolder}'.");
    }
#endif

    void Awake()
    {
        foreach (ConvoSO convo in allConvo)
        {
            GameObject button = Instantiate(convoButton, buttonHolder);
            ConvoHolder convoHolder = button.AddComponent<ConvoHolder>();
            convoHolder.convo = convo;

            button.GetComponentInChildren<TextMeshProUGUI>().text = convo.name;
            button.GetComponentInChildren<Button>().onClick.AddListener(() => convoHolder.StartDialogue());
        }
    }
}

class ConvoHolder : MonoBehaviour
{
    public ConvoSO convo;

    public void StartDialogue()
    {
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(convo);
    }
}