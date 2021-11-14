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

        // remove powerups
        Weapon.powerup1 = false;
        Weapon.powerup2 = false;
        Weapon.powerup3 = false;
        Weapon.powerPoints = 1000;

        Debug.Log("Your total Point: " + Weapon.gamePoints);

        Weapon.gamePoints = 0;

        // reset player direction
        PlayerMovement.facingRight = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
    }
}
