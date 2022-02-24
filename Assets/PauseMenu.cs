using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;

    public static bool isGamePaused = false;

    private float initialTimeScale;

    public void OnPause(InputAction.CallbackContext context) {
        if(context.performed) {
            if(isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause() {
        isGamePaused = true;
        pauseMenuUI.SetActive(true);

        initialTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume() {
        isGamePaused = false;
        pauseMenuUI.SetActive(false);

        Time.timeScale = initialTimeScale;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
