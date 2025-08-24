using System;
using System.IO;
using UnityEngine;

public class TitleDecideIfNewGame : MonoBehaviour
{
    [SerializeField] GameObject newGameBtn;
    [SerializeField] GameObject continueBtn;

    private string savePath;
    private string folderPath;

    void Awake()
    {
        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        savePath = Path.Combine(folderPath, "save.json");


        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        if (!File.Exists(savePath))
        {
            newGameBtn.SetActive(true);
            continueBtn.SetActive(false);
        }
        else
        {
            newGameBtn.SetActive(true);
            continueBtn.SetActive(true);
        }


    }
}
