using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        SceneManager.LoadScene("EventsSandbox");

    }

    public void OnHub()
    {
        SceneManager.LoadScene("HubWorld");

    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
