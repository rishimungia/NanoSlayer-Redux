using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private int health = 100;            // enemy's hit points
    [SerializeField]
    private GameObject deathEffect;      // death effect

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SoundManagerScript.PlaySound("enemyDeath");

        Destroy(gameObject);

        CameraShake.Instance.ShakeCamera(0.3f, 0.4f);
        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}
