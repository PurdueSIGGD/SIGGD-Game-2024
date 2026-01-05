using UnityEngine;
using UnityEngine.InputSystem;

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
        isActive = !menu.activeSelf;
        menu.SetActive(isActive);

        if (isActive)
        {
            Time.timeScale = 0f;
            PlayerID.instance.FreezePlayerMouse();
        }
        else
        {
            Time.timeScale = 1f;
            PlayerID.instance.UnfreezePlayerMouse();
        }
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Closing game");
    }
}
