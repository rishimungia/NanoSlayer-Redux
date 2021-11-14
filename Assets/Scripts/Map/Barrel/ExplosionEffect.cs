using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public int enemyMaxDamage = 100;
    public int playerMaxDamage = 10;
    public float explosionForce;

    private Transform explosionPosition;

    // Start is called before the first frame update
    void Start()
    {
        explosionPosition = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Rigidbody2D body = hitInfo.GetComponent<Rigidbody2D>();
        var forceDirection = body.transform.position - explosionPosition.position;
        float wearOff = 1 - forceDirection.magnitude;
        body.AddForce(forceDirection.normalized * explosionForce * wearOff); 

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) { 
            enemy.TakeDamage(enemyMaxDamage);       // damage enemy
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if (barrel != null) {
            barrel.TakeDamage(2);                   // barrel chain reaction
        }

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null) {
            player.TakeDamage(playerMaxDamage);     // damage player
        }
    }
}
