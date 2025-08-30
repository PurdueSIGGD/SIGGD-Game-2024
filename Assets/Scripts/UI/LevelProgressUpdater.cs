using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUpdater : MonoBehaviour
{
    public static float progress = -1f;
    [SerializeField] bool specificDeactive = false;

    void Update()
    {
        if (specificDeactive)
        {
            GetComponent<Slider>().value = 0;
            Hide();
        }
        else
        {
            if (progress > 0 && EnemiesLeftUpdater.enemiesLeft <= 0)
            {
                GetComponent<Slider>().value = progress;
                Show();
            }
            else
            {
                GetComponent<Slider>().value = 0;
                Hide();
            }
        }
    }

    private void Hide()
    {
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }
    }

    private void Show()
    {
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
        }
    }
}
