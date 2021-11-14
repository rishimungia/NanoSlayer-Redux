using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public HealthUI healthBar;

    private int currentHealth;

    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0) {
            Rigidbody2D player = GetComponent<Rigidbody2D>();
            player.velocity = new Vector2(0f, 0f);
            
            Respawn();
        }
    }

    void Respawn()
    {
        FindObjectOfType<GameManager>().EndGame();
    }
}
