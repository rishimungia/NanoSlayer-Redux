using System.Collections;
using UnityEngine;

public class JumperAI : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float jumpDelay;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private Vector2 attackJump;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform hitCheck;
    [SerializeField]
    private LayerMask targetLayer;

    private bool isNear;
    private bool isGrounded;
    private bool canJump = true;
    private bool canAttack;
    private bool onTarget;

    private Rigidbody2D _rigidBody; // jumper rigid body
    private Transform target;       // target transform
    private PolygonCollider2D polyColloider;

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
        if(canJump && isNear) {
            StartCoroutine("Jump");
        }

        if (_rigidBody.velocity.y < 0) {
            animator.SetBool("IsJumping", false);
        }

        if (!isGrounded && !onTarget && canAttack) {
            float movedistance = Mathf.Abs(_rigidBody.position.x - target.position.x);

            if (_rigidBody.position.x - target.position.x > 0)
                _rigidBody.transform.Translate(new Vector2(-movedistance , 0f) * moveSpeed * Time.deltaTime);
            else
                _rigidBody.transform.Translate(new Vector2(movedistance, 0f) * moveSpeed * Time.deltaTime);
        }

        if (canAttack && onTarget) {
            Attack();
        }

        else if (!canAttack && onTarget) {
            _rigidBody.AddForce(new Vector2(Random.Range(-attackJump.x, attackJump.x), attackJump.y), ForceMode2D.Impulse);
        }

    }

    void FixedUpdate()
    {
        isNear = (Mathf.Abs(_rigidBody.position.x - target.position.x) <= detectionRange) && (Mathf.Abs(_rigidBody.position.y - target.position.y) <= detectionRange);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        onTarget = Physics2D.OverlapCircle(hitCheck.position, 0.05f, targetLayer);
    }

    IEnumerator Jump()
    {
        if (isGrounded && !onTarget) {
            _rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            SoundManagerScript.PlaySound("jump");
            animator.SetBool("IsJumping", true);

            canAttack = true;

            canJump = false;
            yield return new WaitForSeconds(jumpDelay);
            canJump = true;
        }
    }

    void Attack()
    {   
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        player.TakeDamage(attackDamage);
        canAttack = false;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitCheck.position, 0.05f);
    }
}
