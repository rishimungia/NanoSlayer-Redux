using UnityEngine;

public class LavaDeath : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // kills player when fallen into lava
        if (col.name == "Player") {
            PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(100);
            
            FindObjectOfType<GameManager>().EndGame();

            // disables player sliding due to no friction
            Rigidbody2D player = col.GetComponent<Rigidbody2D>();
            player.velocity = new Vector2(0f, 0f);
        }

        // kills enemy when fallen into lava
        if (col.tag == "Enemy") {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(500);
        }

        // explodes barrel when fallen into lava
        if (col.tag == "Barrel") {
            BarrelExplode barrel = col.GetComponent<BarrelExplode>();
            barrel.TakeDamage(2);
        }
    }
}
