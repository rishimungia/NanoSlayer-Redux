using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private GameObject healthBarUI;

    private Slider healthBar;

    private int currentHealth;
    private static bool isInvinsible = false;

    void Start()
    {
        currentHealth = health;
        healthBar = healthBarUI.GetComponent<Slider>();

        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void TakeDamage(int damage, SoundManager.FXSounds sound = SoundManager.FXSounds.Damage)
    {
        if(!isInvinsible) {
            currentHealth -= damage;
            healthBar.value = currentHealth;
            SoundManager.PlaySound(sound);

            if(currentHealth <= 0) {
                Rigidbody2D player = GetComponent<Rigidbody2D>();
                player.velocity = new Vector2(0f, 0f);
                
                Respawn();
            }
        }
    }

    public bool Heal() {
        if(currentHealth < health) {
            currentHealth = health;
            healthBar.value = currentHealth;
            return true;
        }
        else
            return false;
    }

    public static void SetInvincible(bool toggle) {
        isInvinsible = toggle;
    }

    void Respawn()
    {
        FindObjectOfType<GameManager>().EndGame();
    }
}
