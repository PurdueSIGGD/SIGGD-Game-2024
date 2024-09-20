using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * If in doubt of what any of this does or works consult with the META SYSTEMS team ;-;
 * Author: Blobosle
 *
 * Three button functions that link to the current 3 buttons in the main menu.
 */

public class MainMenu_Script : MonoBehaviour
{
    /* All public button functions are called by the UI buttons */
    public void PlayButton(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    public void SettingsButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitButton()
    {
        Application.Quit();
        
        /* Keep this line below to simulate the quit on the Unity editor plsss */
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    /* Used to async load the main play scene */
    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            // We could add a loading progress bar here if we really wanted to
            yield return null;
        }
    }

}
