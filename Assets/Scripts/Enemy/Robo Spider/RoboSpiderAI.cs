using System.Collections;
using UnityEngine;


public class RoboSpiderAI : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float detectionRange = 3.0f;
    [SerializeField]
    private float attackRange = 1.25f;
    [SerializeField]
    private GameObject explodeIndicator;

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

    // Update is called once per frame
    void Update()
    {
        // move close if in detection range
        if (inRange) {
            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else {
                _rigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
        }

        if (inAttackRange) {
            StartCoroutine("Explode");
        }
    }

    void FixedUpdate() {
        targetDistance = Vector2.Distance(_rigidBody.position, target.position);

        inRange = (targetDistance <= detectionRange) && (targetDistance > attackRange);
        
        inAttackRange = (targetDistance <= attackRange);

        animator.SetFloat("Speed", Mathf.Abs(_rigidBody.velocity.x));

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
        
        float normalSpeed = moveSpeed;
        // moveSpeed = 2 * moveSpeed;

        for (int i = 0; i < 2; i++) {
            yield return new WaitForSeconds(0.25f);
            explodeIndicator.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 2.0f;
            yield return new WaitForSeconds(0.25f);
            explodeIndicator.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 8.0f;
        }
        
        if (inAttackRange) {
            FindObjectOfType<Enemy>().Explode();
        }
        else {
            explodeIndicator.SetActive(false);
            // moveSpeed = normalSpeed;
        }
    }
}
