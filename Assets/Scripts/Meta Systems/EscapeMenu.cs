using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    private bool isActive;
    private InputAction escapeAction;

    void OnEnable()
    {
        menu.SetActive(false);
        isActive = false;

        PlayerInput input = PlayerID.instance.GetComponent<PlayerInput>();
        escapeAction = input.actions.FindAction("Escape");
        escapeAction.performed += OnEscapePerformed;
    }

    void OnDisable()
    {
        escapeAction.performed -= OnEscapePerformed;
    }

    private void OnEscapePerformed(InputAction.CallbackContext context)
    {
        isActive = menu.activeSelf;
        if (isActive)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        PlayerID.instance.FreezePlayerMouse();
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        PlayerID.instance.UnfreezePlayerMouse();
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Closing game");
    }

    public void ReturnToHub()
    {
        CloseMenu();
        DeathRingVFX.instance.PlayDeathAnimation();
        PlayerID.instance.gameObject.SetActive(false);
        StartCoroutine(WaitToLoadHub());
    }

    IEnumerator WaitToLoadHub()
    {
        
        ScreenFader.instance.FadeOut(0f, 5);
        AudioManager.Instance.MusicBranch.CrossfadeTo(MusicTrackName.HUB, 2f);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Hubworld");
    }
}
