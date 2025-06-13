using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerDeathManager : MonoBehaviour
{
    Camera cam;
    Animator camAnim;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    public void PlayDeathAnim()
    {
        // toggle camera zoom in animation 
        camAnim.SetBool("isDead", true);
        Time.timeScale = 0.25f;
        // idk what changing the layer does
        gameObject.layer = 0; // I really hope this doesn't collide with anything
        if (GetComponent<PlayerInput>() != null)
        {
            // disable player input
            GetComponent<PlayerInput>().enabled = false;
        }

        float startTime = Time.unscaledTime;
        float endTime = startTime + 3;
        Vector3 originalScale = transform.localScale;

        StartCoroutine(DeathAnimCoroutine(startTime, endTime, originalScale));
    }
    IEnumerator DeathAnimCoroutine(float startTime, float endTime, Vector3 originalScale)
    {
        // perform loop
        while (Time.unscaledTime < endTime)
        {
            float timePercentage = (Time.unscaledTime - startTime) / (endTime - startTime);

            //transform.Rotate(0, 0, Time.unscaledDeltaTime * 360);
            transform.localScale = originalScale * (1 - timePercentage);

            yield return null;
        }
        gameObject.SetActive(false);

        // reset to hub world and reset everything else we changed in this script
        SceneManager.LoadScene("Eva Fractal Hub");
        camAnim.SetBool("isDead", false);
        Time.timeScale = 1;
    }
}
