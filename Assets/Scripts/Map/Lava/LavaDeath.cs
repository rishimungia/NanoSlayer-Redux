using System.Collections;
using UnityEngine;

public class LavaDeath : MonoBehaviour
{
    [SerializeField]
    private float damageDelay;
    [SerializeField]
    private int damageAmountPlayer;
    [SerializeField]
    private int damageAmountEnemy;

    private PlayerHealth playerHealth;
    
    void OnTriggerEnter2D(Collider2D col) {

        // kills player when fallen into lava
        if (col.name == "Player") {
            playerHealth = col.GetComponent<PlayerHealth>();

            StartCoroutine("Burn");           
        }

        // kills enemy when fallen into lava
        if (col.tag == "Enemy") {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(damageAmountEnemy);
        }

        // explodes barrel when fallen into lava
        if (col.tag == "Barrel") {
            BarrelExplode barrel = col.GetComponent<BarrelExplode>();
            barrel.TakeDamage();
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.name == "Player") {
            StopCoroutine("Burn");
        }
    }

    IEnumerator Burn() {
        while(true) {
            CameraShake.Instance.ShakeCamera(0.25f, 0.25f);
            playerHealth.TakeDamage(damageAmountPlayer, SoundManager.FXSounds.LavaDamage);
            yield return new WaitForSeconds(damageDelay);
        }
    }
}
