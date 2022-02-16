using System.Collections;
using UnityEngine;

public class PlantAI : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float attackDelay;

    private bool isActive = false;
    private bool canAttack = false;

    private Transform target;           // target transform
    private PlayerHealth playerHealth;
    private ParticleSystem spawnParticles;
    private Animator animator;  
    private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        spawnParticles = GetComponent<ParticleSystem>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        playerHealth = FindObjectOfType<PlayerHealth>();

        enemyScript = GetComponent<Enemy>();
        enemyScript.enabled = false;
    }

    void FixedUpdate() {
        if(Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer)) {
            if(!isActive) {
                StartCoroutine("Grow");
            }
            else if(canAttack){
                StartCoroutine("Attack");
            }
        }
    }

    IEnumerator Grow() {
        animator.SetTrigger("Grow");
        spawnParticles.Play();
        
        isActive = true;
        
        yield return new WaitForSeconds(0.4f);
        canAttack = true;
        enemyScript.enabled = true;
    }

    IEnumerator Attack() {
        string attackSide = (target.transform.position.x > transform.position.x) ? "AttackRight" : "AttackLeft";
        animator.SetTrigger(attackSide);

        canAttack = false;
        
        yield return new WaitForSeconds(0.3f);      // wait for animation to complete
        if(Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer))   // check if player is still in range before applying damage
            playerHealth.TakeDamage(attackDamage);
        
        yield return new WaitForSeconds(attackDelay - 0.3f);
        canAttack = true;
    }
}
