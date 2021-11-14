using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private int health = 100;            // enemy's hit points
    [SerializeField]
    private int weaponPoints = 500;      // points given for powerup when distroyed
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

        if(!Weapon.powerup1 && !Weapon.powerup2 && !Weapon.powerup3) {
            Weapon.powerPoints = Weapon.powerPoints + weaponPoints;
        }
        if(Weapon.powerPoints>1000) {
            Weapon.powerPoints = 1000;
        }
        Debug.Log("Weapon Points: " + Weapon.powerPoints);
        Weapon.gamePoints = Weapon.gamePoints + 30;         // add 30 points to the total game point.
        Destroy(gameObject);

        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }

    public void Explode() {
        SoundManagerScript.PlaySound("enemyDeath");
        Destroy(gameObject);

        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}
