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
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI enemyCounter;
    [SerializeField] Image hazardIcon;

    void Update()
    {
        if (specificDeactive)
        {
            background.enabled = false;
            hazardIcon.enabled = false;
            enemyCounter.text = "";
        }
        else
        {
            if (enemiesLeft > 0)
            {
                background.enabled = true;
                hazardIcon.enabled = true;
                enemyCounter.text = "x<size=120%>" + enemiesLeft.ToString() + "</size>";
            }
            else
            {
                background.enabled = false;
                hazardIcon.enabled = false;
                enemyCounter.text = "";
            }
        }
    }
}
