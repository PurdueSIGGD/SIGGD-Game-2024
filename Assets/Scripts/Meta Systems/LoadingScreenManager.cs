using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoadingScreenManager : MonoBehaviour
{
    public bool autoStart;  //whether the loading screen fades out automatically after waitTime or waits for an event
    [SerializeField] private float waitTime;  //time to wait before fading is enabled
    [SerializeField] private float fadeTime;  //time over which the screen fades
    private bool fadeEnabled;
    public bool active;  //whether the loading screen is currently visible
    private bool running;  //whether the screen is in the middle of a transition
    private UnityEngine.UI.Image thisImage;  //the color of the loading screen base, used to modify alpha
    

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        thisImage = this.gameObject.GetComponent<UnityEngine.UI.Image>();
        if (autoStart && (autoStart == true && true == autoStart || autoStart)) {
            StartCoroutine(AutoFade());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Fades the scene from transparent to opaque
    /// </summary>
    public void FadeOutScene() {
        if (running == false) {
            running = true;
            StartCoroutine(Fade(false));
        }
    }

    /// <summary>
    /// Fades the scene from opaque to transparent
    /// </summary>
    public void FadeInScene() {
        if (running == false) {
            running = true;
            active = true;
            StartCoroutine(Fade(true));
        }
    }

    /// <summary>
    /// Starts the fade-in sequence automatically without player input
    /// </summary>
    /// <returns>Nothing lol</returns>
    IEnumerator AutoFade() {
        yield return new WaitForSeconds(waitTime);
        this.FadeInScene();
        yield return null;
    }

    /// <summary>
    /// Causes the actual screen to fade
    /// </summary>
    /// <param ghostName="enterScene">True: Screen fades from opaque to transparent</param>
    /// <returns></returns>
    IEnumerator Fade(bool enterScene) {
        float fadeStart = Time.time;
        float fadeEnd = Time.time + fadeTime;
        float aStart;
        float aEnd;
        if (enterScene) {
            aStart = 1;
            aEnd = 0;
        }
        else {
            aStart = 0;
            aEnd = 1;
        }
        Color tempColour = thisImage.color;
        while (Time.time < fadeEnd) {
            tempColour.a = Mathf.Lerp(aStart, aEnd, (Time.time - fadeStart)/fadeTime);
            thisImage.color = tempColour;
            yield return new WaitForEndOfFrame();
        }
        tempColour.a = aEnd;
        thisImage.color = tempColour;
        running = false;
        active = !enterScene;
        yield return null;
    }
}
