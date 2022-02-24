using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private VolumeProfile pauseVolumeProfile;

    private VolumeProfile defaultVolumeProfile;
    private Volume volume;
    

    private GameObject player;
    
    private bool isGameOver = false;

    public static bool isGamePaused = false;

    private float initialTimeScale;
    private float gameTimeScale;

    void Awake() {
        initialTimeScale = Time.timeScale;

        volume = GameObject.FindObjectOfType<Volume>();
        defaultVolumeProfile = volume.profile;

        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
        PlayerMovement.facingRight = true;

        isGamePaused = false;
    }

    public void EndGame ()
    {
        if(isGameOver == false)
        {
            isGameOver = true;

            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            volume.profile = pauseVolumeProfile;

            // disable player mavement and animation
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Animator>().enabled = false;

            // restart the level
            // Invoke("Restart", restartDelay);
        }
    }

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

        volume.profile = pauseVolumeProfile;

        gameTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume() {
        isGamePaused = false;
        pauseMenuUI.SetActive(false);

        volume.profile = defaultVolumeProfile;

        Time.timeScale = gameTimeScale;
    }

    public void Restart() {
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Time.timeScale = initialTimeScale;

        isGamePaused = false;

        // reset player direction
        PlayerMovement.facingRight = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
    }

    public void ExitGame() {
        Time.timeScale = initialTimeScale;
        SceneManager.LoadScene(0);
    }
}
