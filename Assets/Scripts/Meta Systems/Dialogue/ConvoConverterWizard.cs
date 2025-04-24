#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

public class ConvoConverterWizard : ScriptableWizard
{
    public FolderReference targetFolder;
    public TextAsset[] convoFiles;

    [MenuItem("SIGGD/Convo Recipe Wizard")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<ConvoConverterWizard>("Convo Recipe Creator", "Create Convos", "Clear");
    }

    void OnWizardCreate()
    {
        foreach (var convoFile in convoFiles)
        {
            ConvoData data = new ConvoData();
            EditorJsonUtility.FromJsonOverwrite(convoFile.ToString(), data);

            Debug.Log(convoFile.name);
            Debug.Log(data.convoName);

            ConvoSO so = ScriptableObject.CreateInstance<ConvoSO>();
            so.data = data;

            AssetDatabase.CreateAsset(so, $"{targetFolder.Path}/{data.convoName}.asset");
        }
    }

    void OnWizardOtherButton()
    {
        convoFiles = null;
    }

}
#endif