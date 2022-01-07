using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float restartDelay = 1f;
    bool isGameOver = false;

    public void EndGame ()
    {
        if(isGameOver == false)
        {
            isGameOver = true;

            // disable player mavement and animation
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Animator>().enabled = false;

            // restart the level
            Invoke("Restart", restartDelay);
        }
    }

    void Restart ()
    {
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // reset player direction
        PlayerMovement.facingRight = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
    }
}
