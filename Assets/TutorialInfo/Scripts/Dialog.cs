using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

//This class is in charged to manage the Dialog 
public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI textMeshDialog; //The mesh for the Dialog Panel
    public string[] sentences; //Array that stores the sentences
    private string currentSentence; //String that stores the current sentence
    private int index;//Index for the sentence
    public float textSpeed; //This is the speed of the Dialog
    // Start is called before the first frame update
    void Start()
    {
        textMeshDialog.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //
    public void StartDialog()
    {
        index = 0;
        StartCoroutine(Type());
    }

    //Right here you have the Typing process
    IEnumerator Type()
    {
        currentSentence = ""; //We empty the current sentence to avoid overlapping
        currentSentence = sentences[index]; 
        foreach (char letter in currentSentence.ToCharArray())
        {
            textMeshDialog.text += letter;//Here we are adding letter by letter the sentences to the DialogPanel
            yield return new WaitForSeconds(textSpeed);//This allows to display the text with a specific speed

        }

    }
    //Function to pass to Nextline
    public void NextLine()
    {
        if (index < sentences.Length - 1)
        {
            StopAllCoroutines();
            index++;
            textMeshDialog.text = string.Empty; //We empty the panel to avoid overlapping
            StartCoroutine(Type()); //We call Type
        }
        else
        {
            EndDialog();//If there aren't sentences left we end the dialog
        }
    }

    //Function to end Dialog
    private void EndDialog()
    {
        gameObject.SetActive(false);
    }
}
