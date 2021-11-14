using UnityEngine;

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
    [SerializeField]
    private Transform shotgunPoint;
    [SerializeField]
    private Transform shotgunFirePoint;

    public static bool facingRight = true;

    private Animator animator;              // player animator 
    private Rigidbody2D _rigidBody;         // player rigidbody
    private BoxCollider2D boxColloider;     // player box colloider
    
    private bool isGrounded;
    private bool touchingCiling;
    private bool isTouchingWall;

    private bool wallSliding = false;
    private bool isCrouching = false;

    private int availableJumps;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        
        boxColloider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // to make player slide from wall
        if(wallSliding) {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, wallSlidingSpeed, float.MaxValue));
        }

        // reset jumps if player is grounded
        if(isGrounded || wallSliding) {
            availableJumps = extraJumps;
        }
            
        // jump
        if(Input.GetButtonDown("Jump") && availableJumps > 0 && !touchingCiling && !wallSliding) {  // for extra jumps
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            availableJumps--;

            SoundManagerScript.PlaySound("jump");
            animator.SetBool("IsJumping", true); // triggers jump animation
        }

        else if(Input.GetButtonDown("Jump") && availableJumps == 0 && isGrounded && !touchingCiling && !wallSliding) {  // for single/last jump
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);

            animator.SetBool("IsJumping", true); // triggers jump animation
        }

        else if(Input.GetButtonDown("Jump") && wallSliding) {   // jump after wall sliding
            _rigidBody.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            _rigidBody.transform.Translate(new Vector2(wallJumpDistance, 0f) * Time.deltaTime); // move away from wall

            animator.SetBool("IsJumping", true); // triggers jump animation
            SoundManagerScript.PlaySound("jump");
        }

        if(_rigidBody.velocity.y <= 0.01 && isGrounded) {
            animator.SetBool("IsJumping", false); // ends jumping animation
        }

        // crouch
        if(Input.GetButtonDown("Crouch") && !wallSliding) {
            isCrouching = true;
            UpdateFirePoint(0f, -0.21f);    // update fire point while crouching
            animator.SetBool("IsCrouching", true);
        }
        else if((Input.GetButtonUp("Crouch") && !touchingCiling && isCrouching) || (!Input.GetButton("Crouch") && isCrouching && !touchingCiling)) {
            isCrouching = false;
            UpdateFirePoint(0f, 0.21f);     // reset fire point after crouching
            animator.SetBool("IsCrouching", false);
        }

        UpdatePlayerColloider();    // updates player colloider while standing / crouching / wall-sliding
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
            if(!wallSliding) {
                if(facingRight)     // update fire point while wall sliding
                    UpdateFirePoint(0.08f, -0.03f);
                else
                    UpdateFirePoint(-0.08f, -0.03f);
            }

            wallSliding = true;

            animator.SetBool("IsWallGrabbing", true);
            animator.SetBool("IsJumping", false);
        }
        else {
            if(wallSliding) {
                if(facingRight)     // reset fire point after wall sliding
                    UpdateFirePoint(-0.08f, 0.03f);
                else
                    UpdateFirePoint(0.08f, 0.03f);
            }

            wallSliding = false;

            animator.SetBool("IsWallGrabbing", false);

            if(!isGrounded) {
                animator.SetBool("IsJumping", true);
            }
        }
        
        // movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if(!isCrouching) {
            _rigidBody.velocity = new Vector2(moveHorizontal * moveSpeed, _rigidBody.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal)); // trigger run animation 
        }
        else {
            _rigidBody.velocity = new Vector2(moveHorizontal * crouchedMoveSpeed, _rigidBody.velocity.y);
        }
        
        // flipping when necessary
        if(!facingRight && moveHorizontal > 0 && !wallSliding || facingRight && moveHorizontal < 0 && !wallSliding)
            Flip();
    }
    
    // function to flip character
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // function to update player box colloider
    void UpdatePlayerColloider()
    {
        if(isCrouching) {
            boxColloider.size = new Vector2(0.2354203f, 0.3754354f);
            boxColloider.offset = new Vector2(0.0393582f, -0.1872717f);
        }
        else if(wallSliding) {
            boxColloider.size = new Vector2(0.2840797f, 0.498189f);
            boxColloider.offset = new Vector2(0.01508027f, -0.07814106f);
        }
        else {
            boxColloider.size = new Vector2(0.2189612f, 0.5777787f);
            boxColloider.offset = new Vector2(0.06211042f, -0.08610004f);
        }
    }

    void UpdateFirePoint(float x, float y)
    {
        firePoint.position = new Vector2(firePoint.position.x + x, firePoint.position.y + y);
        shotgunFirePoint.position = new Vector2(shotgunFirePoint.position.x + x, shotgunFirePoint.position.y + y);
        shotgunPoint.position = new Vector2(shotgunPoint.position.x + x, shotgunPoint.position.y + y);
    }
}
