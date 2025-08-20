using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiritTrackerCanvasUI : MonoBehaviour
{
    public static SpiritTrackerCanvasUI Instance { get; private set; }   // allows read-only access to the PersistentData instance
    [SerializeField] List<SpiritCounterUI> spiritCounters;
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);   // destroys any duplicate instances of PersistentData
        }

        SceneManager.activeSceneChanged += SwitchCounterType;
    }

    void SwitchCounterType(Scene s0, Scene s1)
    {
        bool fromSaveManager = s1.name.Equals("Eva Start Fractal Hub");

        foreach (SpiritCounterUI counter in spiritCounters)
        {
            counter.fromSaveManager = fromSaveManager;
        }

        UpdateCounters();
    }

    public void UpdateCounters()
    {
        foreach (SpiritCounterUI ui in spiritCounters)
        {
            ui.UpdateText();
        }
    }
}
