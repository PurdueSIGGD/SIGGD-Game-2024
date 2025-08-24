using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrePrologueSceneManager : MonoBehaviour
{
    [SerializeField] Image title;
    [SerializeField] Image starryBG;
    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] string sceneString;
    [SerializeField] TextMeshProUGUI eonsLater;

    float titleFadeInSpeed = 0;
    float titleFadeOutSpeed = 0;
    float bgFadeOutSpeed = 0;
    float skipTextFadeOutSpeed = 0;
    float dialogueFadeOutSpeed = 0;
    float eonsFadeInSpeed = 0;

    void Start()
    {
        StartCoroutine(LoadIntoPrologueHub());
        StartCoroutine(StartFadeInTitle());
        StartCoroutine(StartFadeOutTitle());
        StartCoroutine(StartFadeOutBG());
        StartCoroutine(StartFadeOutSkipText());
        StartCoroutine(StartFadeOutDialogue());
        StartCoroutine(StartFadeInEonsLater());
    }


    void Update()
    {
        if (FadeInItem(title, titleFadeInSpeed))
        {
            FadeOutItem(title, titleFadeOutSpeed);
        }
        FadeInItem(eonsLater, eonsFadeInSpeed);
        FadeOutItem(starryBG, bgFadeOutSpeed);
        FadeOutItem(skipText, skipTextFadeOutSpeed);
        FadeOutItem(dialogue, dialogueFadeOutSpeed);
    }

    IEnumerator LoadIntoPrologueHub()
    {
        yield return new WaitForSeconds(69f);
        SceneManager.LoadScene("Prologue_Hubworld");
    }

    IEnumerator StartFadeInEonsLater()
    {
        yield return new WaitForSeconds(66.5f);
        eonsFadeInSpeed = 0.4f;
    }

    IEnumerator StartFadeInTitle()
    {
        yield return new WaitForSeconds(59);
        titleFadeInSpeed = 0.1f;
    }

    IEnumerator StartFadeOutTitle()
    {
        yield return new WaitForSeconds(65);
        titleFadeOutSpeed = 0.4f;
    }

    IEnumerator StartFadeOutBG()
    {
        yield return new WaitForSeconds(65);
        bgFadeOutSpeed = 0.35f;
    }

    IEnumerator StartFadeOutSkipText()
    {
        yield return new WaitForSeconds(2);
        skipTextFadeOutSpeed = 0.1f;
    }

    IEnumerator StartFadeOutDialogue()
    {
        yield return new WaitForSeconds(55);
        dialogueFadeOutSpeed = 1;
    }

    private bool FadeInItem(Image image, float speed)
    {
        if (image.color.a >= 1)
        {
            return true;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b,
            image.color.a + speed * Time.deltaTime);
        return false;
    }
    private void FadeInItem(TextMeshProUGUI text, float speed)
    {
        if (text.color.a >= 1)
        {
            return;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b,
            text.color.a + speed * Time.deltaTime);
    }

    private void FadeOutItem(Image image, float speed)
    {
        if (image.color.a <= 0)
        {
            return;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b,
            image.color.a - speed * Time.deltaTime);
    }

    private void FadeOutItem(TextMeshProUGUI text, float speed)
    {
        if (text.color.a <= 0)
        {
            return;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b,
            text.color.a - speed * Time.deltaTime);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    // Fade in title card at the end
    //    if (needFadeInTitle == true && title.color.a < 1)
    //    {
    //        title.color = new Color(title.color.r, title.color.g, title.color.b,
    //            title.color.a + titleFadeInSpeed * Time.deltaTime);
    //    }
    //    else if (title.color.a >= 1)
    //    {
    //        needFadeOut = true;
    //    }

    //    // First fade in the skip text box
    //    if (needSkipFade && tempColor.a < 1)
    //    {
    //        skipText.color = new Color(tempColor.r, tempColor.g, tempColor.b,
    //                    tempColor.a + skipFadeInSpeed * Time.deltaTime);
    //        tempColor = skipText.color;

    //        if (tempColor.a >= 1)
    //        {
    //            needFadeIn = true;
    //            needSkipFade = false;
    //            tempColor = starryBG.color;
    //        }
    //    }

    //    // Fade in starry BG
    //    if (needFadeIn && tempColor.a < 1)
    //    {
    //        starryBG.color = new Color(tempColor.r, tempColor.g, tempColor.b,
    //                    tempColor.a + fadeInSpeed * Time.deltaTime);
    //        tempColor = starryBG.color;

    //        if (tempColor.a >= 1)
    //        {
    //            needFadeIn = false;
    //        }
    //    }

    //    // Fade out starry BG and text
    //    if (needFadeOut && tempColor.a > 0)
    //    {
    //        starryBG.color = new Color(starryBG.color.r, starryBG.color.g, starryBG.color.b,
    //                    tempColor.a - fadeInSpeed * Time.deltaTime);
    //        skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b,
    //                    tempColor.a - fadeInSpeed * Time.deltaTime);
    //        dialogue.color = new Color(dialogue.color.r, dialogue.color.g, dialogue.color.b,
    //                    tempColor.a - fadeInSpeed * Time.deltaTime);
    //        tempColor = starryBG.color;

    //        if (tempColor.a <= 0)
    //        {
    //            SceneManager.LoadScene(sceneString);
    //        }
    //    }

    //    if (dialogue.transform.localPosition.y >= 1100)
    //    {
    //        needFadeInTitle = true;
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        needFadeOut = true;
    //    }
    //}
}
