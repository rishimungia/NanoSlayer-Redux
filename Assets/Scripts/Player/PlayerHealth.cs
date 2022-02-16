using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public HealthUI healthBar;

    private int currentHealth;
    private static bool isInvinsible = false;

    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
        if(!isInvinsible) {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

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
            healthBar.SetHealth(currentHealth);
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
