using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;         // weapon fire point
    [SerializeField]
    private GameObject bulletPrefab;     // bullet prefab

    [SerializeField]
    private float firingRate;

    private bool canShoot = true;
    
    public static int powerPoints = 1000;       // amount of weapon points
    public static int gamePoints = 0;           // The points collected throughout the game
    public static int bulletHitPoits = 0;       // Number of times bullet hit the enemy

    private Animator animator;           // for controlling animation

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine("Fire");
        }
    }
    IEnumerator Fire()
    {
        canShoot = false;

        var shootobj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        shootobj.GetComponent<Bullet>().FireStart();

        animator.SetTrigger("Shoot");
        CameraShake.Instance.ShakeCamera(0.1f, 0.1f);
        
        yield return new WaitForSeconds(firingRate);
        canShoot = true;
    }
}
