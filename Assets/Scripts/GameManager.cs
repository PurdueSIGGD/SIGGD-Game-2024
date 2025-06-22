using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool GUIshutdown;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHome()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnGame()
    {
        // Prevent player from spamming button while sfx is playing
        if (GUIshutdown == false)
        {
            GUIshutdown = true;
            // Pause for SFX before transition
            Invoke("PrePrologue", 3);
        }

    }

    public void OnCredit()
    {
        // Prevent player from spamming button while sfx is playing
        if (GUIshutdown == false)
        {
            GUIshutdown = true;
            // Pause for SFX before transition
            Invoke("Credits", 3);
        }
    }

    public void OnHub()
    {
        SceneManager.LoadScene("HubWorld");

    }

    public void OnQuit()
    {
        Application.Quit();
    }


    void PrePrologue()
    {
        GUIshutdown = false;
        SceneManager.LoadScene("PrePrologue");
    }
    void Credits()
    {
        GUIshutdown = false;
        SceneManager.LoadScene("Credits");
    }
}
