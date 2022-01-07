using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;             // bullet travel speed

    public float knockbackX = 1.0f;         // bullet knockback multiplier in x axis
    public float knockbackY = 1.0f;         // bullet knockback multiplier in y axis

    public int damage = 40;                 // bullet damage amount
    public GameObject impactEffect;         // bullet impact effect
    
    private Rigidbody2D _rigidbody;
    
    public void FireStart()
    {
        SoundManagerScript.PlaySound("shoot");
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * speed;  // give bullet const velocity
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // if bullet hits an enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            SoundManagerScript.PlaySound("bulletImpact");
            enemy.TakeDamage(damage);       // damage enemy
            Weapon.bulletHitPoits++;
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if(barrel != null)
        {
            barrel.TakeDamage();
        }

        // bullet knockback
        if(hitInfo.tag == "Barrel" || hitInfo.tag == "Enemy")
        {
            Rigidbody2D prop = hitInfo.gameObject.GetComponent<Rigidbody2D>();
            prop.AddForce(new Vector2(_rigidbody.velocity.x * knockbackX, knockbackY), ForceMode2D.Impulse);
        }

        Destroy(gameObject);                // destroy bullet prefab

        GameObject bulletImpact = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(bulletImpact, 0.4f);
    }
}
