using System.Collections;
using UnityEngine;


public class RoboSpiderAI : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float detonationDelay;
    [SerializeField]
    private GameObject explodeIndicator;
    [SerializeField]
    private GameObject explosionPrefab;

    private bool facingRight = false;
    private float targetDistance;
    private bool inRange;
    private bool inAttackRange;

    private Rigidbody2D _rigidBody;
    private Transform target;
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        targetDistance = Vector2.Distance(_rigidBody.position, target.position);

        inRange = (targetDistance <= detectionRange) && (targetDistance > attackRange);
        
        inAttackRange = (targetDistance <= attackRange);

        animator.SetFloat("Speed", Mathf.Abs(_rigidBody.velocity.x));

        // move close if in detection range
        if (inRange) {
            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(-moveSpeed, _rigidBody.velocity.y);
            }
            else {
                _rigidBody.velocity = new Vector2(moveSpeed, _rigidBody.velocity.y);
            }
        }

        if (inAttackRange) {
            StartCoroutine("Explode");
        }

        // flipping when necessary
        if(!facingRight && _rigidBody.velocity.x > 0 || facingRight && _rigidBody.velocity.x < 0)
            Flip();
        else if(facingRight && _rigidBody.velocity.x == 0 && _rigidBody.position.x - target.position.x > 0 || !facingRight && _rigidBody.velocity.x == 0 && _rigidBody.position.x - target.position.x < 0)
            Flip();
    }

    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator Explode() {
        explodeIndicator.SetActive(true);

        SoundManager.PlaySound(gameObject.GetComponent<AudioSource>(), SoundManager.EnemySounds.SpiderIgnite, true);

        yield return new WaitForSeconds(detonationDelay);
        
        if (inAttackRange) {
            Detonate();
        }
        else {
            explodeIndicator.SetActive(false);
            SoundManager.PlaySound(gameObject.GetComponent<AudioSource>(), SoundManager.EnemySounds.SpiderIgnite, false);
        }

    }

    void Detonate() {
        Destroy(gameObject);
        SoundManager.PlaySound(SoundManager.EnemySounds.SpiderExplode, transform.position);

        CameraShake.Instance.ShakeCamera(0.2f, 0.4f);
        GameObject deathEffectObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffectObject, 0.4f);
    }
}
