using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesLeftUpdater : MonoBehaviour
{
    public static int enemiesLeft = -1;
    [SerializeField] bool specificDeactive;

    void Update()
    {
        if (specificDeactive)
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            if (enemiesLeft > 0)
            {
                GetComponent<TextMeshProUGUI>().text = "Enemies Left: " + enemiesLeft.ToString();
            }
            else
            {
                GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}
