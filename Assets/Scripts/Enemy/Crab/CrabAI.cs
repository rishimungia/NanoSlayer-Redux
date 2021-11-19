using System.Collections;
using UnityEngine;

public class CrabAI : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float detectionRange = 3.0f;
    [SerializeField]
    private float attackRange = 1.25f;
    [SerializeField]       
    private float escapeRange = 0.75f;

    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private float attackDelay = 1.0f;

    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Transform frontCheck;
    [SerializeField]
    private GameObject bulletPrefab;

    private bool facingRight = false;
    private bool facingTarget;

    private float targetDistance;
    private bool inRange;
    private bool inEscapeRange;
    private bool canMove;
    private bool inAttackRange;
    private bool canShoot = true;

    private Rigidbody2D _rigidBody;     // crab rigid body
    private Transform target;           // target transform
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
        if (inAttackRange && canShoot && facingTarget) {
            StartCoroutine("Shoot");
        }

        // move close if in detection range
        if (inRange && !inEscapeRange && !inAttackRange) {
            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else {
                _rigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
        }

        // move away if in escape range
        else if (inEscapeRange && canMove) {
            if (_rigidBody.position.x - target.position.x > 0) {
                _rigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
            else {
                _rigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }

        }

    }

    void FixedUpdate()
    {
        targetDistance = Vector2.Distance(_rigidBody.position, target.position);

        inRange = (targetDistance <= detectionRange) && (targetDistance > attackRange);

        inEscapeRange = targetDistance <= escapeRange;

        canMove = !Physics2D.OverlapCircle(frontCheck.position, 0.24f, wallLayer);

        inAttackRange = (targetDistance <= attackRange);

        Debug.DrawRay(firePoint.position, transform.forward, Color.green);

        if (facingRight)
            facingTarget = Physics2D.Raycast(firePoint.position, Vector2.right, detectionRange, targetLayer);
        else
            facingTarget = Physics2D.Raycast(firePoint.position, -Vector2.right, detectionRange, targetLayer);

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

    // function to shoot
    IEnumerator Shoot() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        canShoot = false;
        yield return new WaitForSeconds(attackDelay);
        canShoot = true;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(frontCheck.position, 0.24f);
    }
}