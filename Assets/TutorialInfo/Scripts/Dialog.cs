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

    public TextMeshProUGUI textMeshDialog;
    public GameObject textMeshDialogGameObject;
    public string[] sentences;
    private int index;
    public float textSpeed;
    // Start is called before the first frame update
    void Start()
    {
        textMeshDialogGameObject=GameObject.Find("DialogPanel");
        textMeshDialog.text=string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void StartDialog(){
        Debug.Log("StartDialog");
        textMeshDialogGameObject.SetActive(true);
        index=0;
        StartCoroutine(Type());
    }

    //Right here you 
    IEnumerator Type(){
        foreach(char letter in sentences[index].ToCharArray()){
              textMeshDialog.text+=letter;
              yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine(){
        if(index < sentences.Length-1){
            index++;
            textMeshDialog.text=string.Empty;
            StartCoroutine(Type());
        }
        else{
            EndDialog();
        }
    }

    public void TestButton(){
        Debug.Log("Test");
    }
    private void EndDialog(){
        gameObject.SetActive(false);
    }
}
