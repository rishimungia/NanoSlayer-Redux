using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;                     // bullet travel speed

    [SerializeField]
    private float knockbackX;               // bullet knockback multiplier in x axis
    [SerializeField]
    private float knockbackY;               // bullet knockback multiplier in y axis

    [SerializeField]
    private int damage;                     // bullet damage amount
    [SerializeField]
    private GameObject impactEffect;         // bullet impact effect

    [SerializeField]
    private float bulletLifetime;
    
    private Rigidbody2D _rigidbody;

    void Start() {
        Destroy(gameObject, bulletLifetime);
    }
    
    public void FireStart() {
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
