using UnityEngine;

public class CrabShoot : MonoBehaviour
{
    public float speed = 2.0f;
    public int damage = 15;

    public GameObject impactEffect;
    
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _rigidbody.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if(player != null) {
            player.TakeDamage(damage);      // damage player
        }

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            SoundManagerScript.PlaySound("bulletImpact");
            enemy.TakeDamage(damage);       // damage enemy
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if(barrel != null)
        {
            barrel.TakeDamage(1);
        }

        Destroy(gameObject);
        GameObject bulletImpact = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(bulletImpact, 0.4f);
    }
}
