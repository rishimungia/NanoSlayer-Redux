using UnityEngine;

public class PowerDashEffector : MonoBehaviour
{
    [SerializeField]
    private int powerDashDamage;
    
    void OnTriggerEnter2D(Collider2D hitInfo) {
        // if player hits an enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            SoundManagerScript.PlaySound("bulletImpact");
            enemy.TakeDamage(powerDashDamage);       // damage enemy
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if(barrel != null)
        {
            barrel.TakeDamage();
        }
    }
}
