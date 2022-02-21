using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D powerDashEffector;
    [SerializeField]
    private float powerDashSpeed;
    [SerializeField]
    private float powerDashCooldown;

    private Animator animator;
    private Rigidbody2D _rigidBody;

    private float originalGravity;

    private bool canPowerDash = true;

    // ablities status
    public static bool powerDashActive = false;

    // Start is called before the first frame update
    void Start() {

        animator = GetComponent<Animator>();

        _rigidBody = GetComponent<Rigidbody2D>();

        originalGravity = _rigidBody.gravityScale;

        // disable all abilities
        powerDashEffector.enabled = false;
    }

    public void OnPowerDash(InputAction.CallbackContext context) {
        if (context.performed && PlayerMovement.canDash && canPowerDash) {
            StartCoroutine("PowerDash");
        }
    }

    // power dash
    IEnumerator PowerDash() {
        powerDashActive = true;
        canPowerDash = false;
        powerDashEffector.enabled = true;
        PlayerHealth.SetInvincible(true);

        PlayerMovement.canDash = false;
        PlayerMovement.canCrouch = false;
        PlayerMovement.canMove = false;
        PlayerMovement.canJump = false;
        Weapon.DisableWeapon(true);

        animator.SetBool("IsPowerDashing", powerDashActive);
        SoundManager.PlaySound(SoundManager.PlayerSounds.PlayerPowerDash);
        
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0.0f);

        float defaultDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = defaultDeltaTime * Time.timeScale;

        CameraShake.Instance.ShakeCamera(0.7f, 0.5f);
        
        if (PlayerMovement.facingRight) {
            _rigidBody.AddForce(new Vector2(powerDashSpeed, 0.0f), ForceMode2D.Impulse);
        }
        else {
            _rigidBody.AddForce(new Vector2(-powerDashSpeed, 0.0f), ForceMode2D.Impulse);
        }
        
        _rigidBody.gravityScale = 0.0f;

        yield return new WaitForSeconds(0.5f);

        powerDashActive = false;
        powerDashEffector.enabled = false;
        PlayerHealth.SetInvincible(false);
        animator.SetBool("IsPowerDashing", powerDashActive);

        PlayerMovement.canDash = true;
        PlayerMovement.canCrouch = true;
        PlayerMovement.canMove = true;
        PlayerMovement.canJump = true;
        Weapon.DisableWeapon(false);

        _rigidBody.gravityScale = originalGravity;

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = defaultDeltaTime * Time.timeScale;

        yield return new WaitForSeconds(powerDashCooldown);
        canPowerDash = true;
    }
}
