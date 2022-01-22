using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;         // movement speed of player
    [SerializeField]
    private float crouchedMoveSpeed = 5.0f;  // movement speed when crouched
    [SerializeField]
    private float jumpHeight = 5.0f;         // jump force of the player
    [SerializeField]
    private float wallJumpDistance = 25.0f;  // distance to move player when jumping from wall
    [SerializeField]
    private int extraJumps;                  // number of allowed extra jumps
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashCooldown;
    
    [SerializeField][Range (0.0f, 1.0f)]
    private float wallSlidingSpeed;          // wall sliding speed

    [SerializeField]
    private Transform groundCheck;           // to check if player is touching ground
    [SerializeField]
    private Transform cilingCheck;           // to check if player under a ciling
    [SerializeField]
    private Transform frontCheck;            // to check if player is grabbing a wall
    [SerializeField]
    private Transform rearCheck;

    [SerializeField]
    private LayerMask groundLayer;           // what is ground
    [SerializeField]
    private LayerMask wallLayer;             // what is wall

    [SerializeField]
    private Transform firePoint;             // bullet fire-point

    public static bool facingRight = true;

    private Animator animator;              // player animator 
    private Rigidbody2D _rigidBody;         // player rigidbody

    private float moveHorizontal;
    
    private bool isGrounded;
    private bool touchingCiling;
    private bool isTouchingWall;

    private bool wallSliding = false;
    private bool isCrouching = false;
    private bool isDashing = false;
    private bool canDash = true;

    private int availableJumps;
    private float originalGravity;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        originalGravity = _rigidBody.gravityScale;
    }

    void FixedUpdate()
    {
        
        // check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // check if the player's head is touching ciling
        touchingCiling = Physics2D.OverlapCircle(cilingCheck.position, 0.1f, groundLayer);

        // check if the player is touching a wall
        if(Physics2D.OverlapCircle(frontCheck.position, 0.1f, wallLayer)) {
            Flip();
            isTouchingWall = true;
        }
        else if(Physics2D.OverlapCircle(rearCheck.position, 0.1f, wallLayer)) {
            isTouchingWall = true;
        }  
        else
            isTouchingWall = false;

        // trigger wall sliding
        if(isTouchingWall && !isGrounded) {
            wallSliding = true;

            animator.SetBool("IsWallGrabbing", true);
            animator.SetBool("IsJumping", false);
        }
        else {
            wallSliding = false;

            animator.SetBool("IsWallGrabbing", false);

            if(!isGrounded) {
                animator.SetBool("IsJumping", true);
            }
        }
        
        // movement
        if (!wallSliding && !isDashing) {
            if (!isCrouching) {
                _rigidBody.velocity = new Vector2(moveHorizontal * moveSpeed, _rigidBody.velocity.y);
                animator.SetFloat("Speed", Mathf.Abs(moveHorizontal)); // trigger run animation 
            }
            else {
                _rigidBody.velocity = new Vector2(moveHorizontal * crouchedMoveSpeed, _rigidBody.velocity.y);
            }
        }

        // to make player slide from wall
        if (wallSliding && !isCrouching) {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else if (wallSliding && isCrouching) {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, -wallSlidingSpeed*2, float.MaxValue));
        }
        

        // reset jumps if player is grounded
        if(isGrounded || wallSliding) {
            availableJumps = extraJumps;
        }
            
        // reset jump animation
        if(Mathf.Abs(_rigidBody.velocity.y) <= 0.01) {
            animator.SetBool("IsJumping", false); // ends jumping animation
        }

        // flipping when necessary
        if(!facingRight && moveHorizontal > 0 && !wallSliding || facingRight && moveHorizontal < 0 && !wallSliding)
            Flip();

    }

    // movement input
    public void OnMove (InputAction.CallbackContext context) {
        if(context.ReadValue<float>() > 0)
            moveHorizontal = 1;
        else if(context.ReadValue<float>() < 0)
            moveHorizontal = -1;
        else
            moveHorizontal = 0;
    }
    
    // jump
    public void OnJump (InputAction.CallbackContext context) {
        if(context.performed && availableJumps > 0 && !touchingCiling && !wallSliding) {  // for extra jumps
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            availableJumps--;

            SoundManagerScript.PlaySound("jump");
            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        else if(context.performed && availableJumps == 0 && isGrounded && !touchingCiling && !wallSliding) {  // for single/last jump
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

            animator.SetBool("IsJumping", true); // triggers jump animation
        }
        else if(context.performed && wallSliding) {   // jump after wall sliding
            if (facingRight)
                _rigidBody.AddForce(new Vector2(wallJumpDistance, jumpHeight), ForceMode2D.Impulse);
            else
                _rigidBody.AddForce(new Vector2(-wallJumpDistance, jumpHeight), ForceMode2D.Impulse);

            animator.SetBool("IsJumping", true); // triggers jump animation
            SoundManagerScript.PlaySound("jump");
        }
    }

    // crouch
    public void OnCrouch(InputAction.CallbackContext context) {
        if(context.performed && !wallSliding) {
            isCrouching = true;
            animator.SetBool("IsCrouching", true);
        }
        else if(context.canceled && !touchingCiling && isCrouching) {
            isCrouching = false;
            animator.SetBool("IsCrouching", false);
        }
    }

    // dash
    public void OnDash(InputAction.CallbackContext context) {
        if(context.performed && canDash && !isCrouching && !wallSliding) {
            StartCoroutine("Dash");
        }
    }

    IEnumerator Dash() {
        isDashing = true;
        canDash = false;
        animator.SetBool("IsDashing", isDashing);
        
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0.0f);

        if (facingRight) {
            _rigidBody.AddForce(new Vector2(dashSpeed, 0.0f), ForceMode2D.Impulse);
        }
        else {
            _rigidBody.AddForce(new Vector2(-dashSpeed, 0.0f), ForceMode2D.Impulse);
        }
        
        _rigidBody.gravityScale = 0.0f;

        yield return new WaitForSeconds(0.2f);

        isDashing = false;
        animator.SetBool("IsDashing", isDashing);

        _rigidBody.gravityScale = originalGravity;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}