using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    private int enemyMaxDamage = 100;
    [SerializeField]
    private int playerMaxDamage = 10;
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private float explosionForcePlayer;

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

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) { 
            enemy.TakeDamage(enemyMaxDamage);       // damage enemy
            body.AddForce(forceDirection.normalized * explosionForce * wearOff, ForceMode2D.Impulse); 
            PlayerAbilities.AddPowerPoints(10);
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if (barrel != null) {
            barrel.TakeDamage();                   // barrel chain reaction
            body.AddForce(forceDirection.normalized * explosionForce * wearOff, ForceMode2D.Impulse); 
            PlayerAbilities.AddPowerPoints(5);
        }

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null) {
            player.TakeDamage(playerMaxDamage);     // damage player
            body.AddForce(new Vector2(explosionForcePlayer * wearOff, 0.0f), ForceMode2D.Impulse); 
        }
    }
}
