using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PrePrologueSceneManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer starryBG;
    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] TextMeshProUGUI dialogue;
    [SerializeField] string sceneString;

    private bool needSkipFade = true;
    private bool needFadeIn = false;
    private bool needFadeOut = false;
    private float skipFadeInSpeed = 0.5f;
    private float fadeInSpeed = 1f;
    public float fadeOutSpeed = 1f;
    private Color tempColor;

    private float startTime;
    private float holdTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        tempColor = skipText.color;
    }

    // Update is called once per frame
    void Update()
    {
        // First fade in the skip text box
        if (needSkipFade && tempColor.a < 1)
        {
            skipText.color = new Color(tempColor.r, tempColor.g, tempColor.b,
                        tempColor.a + skipFadeInSpeed * Time.deltaTime);
            tempColor = skipText.color;

            if (tempColor.a >= 1)
            {
                needFadeIn = true;
                needSkipFade = false;
                tempColor = starryBG.color;
            }
        }

        // Fade in starry BG
        if (needFadeIn && tempColor.a < 1)
        {
            starryBG.color = new Color(tempColor.r, tempColor.g, tempColor.b,
                        tempColor.a + fadeInSpeed * Time.deltaTime);
            tempColor = starryBG.color;

            if (tempColor.a >= 1)
            {
                needFadeIn = false;
            }
        }

        // Fade out starry BG and text
        if (needFadeOut && tempColor.a > 0)
        {
            starryBG.color = new Color(starryBG.color.r, starryBG.color.g, starryBG.color.b,
                        tempColor.a - fadeInSpeed * Time.deltaTime);
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b,
                        tempColor.a - fadeInSpeed * Time.deltaTime);
            dialogue.color = new Color(dialogue.color.r, dialogue.color.g, dialogue.color.b,
                        tempColor.a - fadeInSpeed * Time.deltaTime);
            tempColor = starryBG.color;

            if (tempColor.a <= 0)
            {
                SceneManager.LoadScene(sceneString);
            }
        }

        if (dialogue.transform.localPosition.y >= 2550)
        {
            Debug.Log("REACHED???");
            needFadeOut = true;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    startTime = Time.time;
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (startTime + holdTime <= Time.time)
        //    {
        //        needFadeOut = true;
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            needFadeOut = true;
        }
    }
}
