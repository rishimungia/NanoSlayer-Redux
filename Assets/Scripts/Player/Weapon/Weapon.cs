using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;         // weapon fire point
    [SerializeField]
    private GameObject bulletPrefab;     // bullet prefab

    [SerializeField]
    private float fireDelay;

    private bool canShoot = true;
    private bool weaponInput;

    public static bool weaponsDisabled = false;

    private Animator animator;           // for controlling animation

    void Start() {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (canShoot && weaponInput) {
            StartCoroutine("Fire");
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        weaponInput = context.performed;
    }

    IEnumerator Fire()
    {
        if(!weaponsDisabled) {
            canShoot = false;
            
            var shootobj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootobj.GetComponent<Bullet>().FireStart();

            animator.SetTrigger("Shoot");
            CameraShake.Instance.ShakeCamera(0.1f, 0.1f);
            
            yield return new WaitForSeconds(fireDelay);
            canShoot = true;
        }
    }

    public static void DisableWeapon(bool toggle) {
        weaponsDisabled = toggle;
    }
}
