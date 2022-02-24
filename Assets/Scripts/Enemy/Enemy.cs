using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private int health = 100;            // enemy's hit points
    [SerializeField]
    private GameObject deathEffect;      // death effect
    [SerializeField]
    private bool invincible = false;
    
    [SerializeField]
    public int hitScore = 2;
    [SerializeField]
    private int killScore = 5;

    private int playerHits = 0;

    public void TakeDamage(int damage, bool playerHit = false)
    {
        if(!invincible)
            health -= damage;

        if(playerHit) 
            playerHits += 1;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SoundManager.PlaySound(SoundManager.EnemySounds.GenericEnemyDeath, transform.position);

        PlayerAbilities.AddPowerPoints(playerHits * 5);
        ScoreManager.Instance.AddScorePoint(killScore);

        Destroy(gameObject);

        CameraShake.Instance.ShakeCamera(0.3f, 0.4f);
        GameObject deathEffectObject = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}
