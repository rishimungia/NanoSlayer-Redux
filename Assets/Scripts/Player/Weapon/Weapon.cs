using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;         // weapon fire point
    public Transform ShotgunPoint;      // shotgun placeholder
    public Transform ShotgunFirePoint;  // shotgun fire point
    public GameObject bulletPrefab;     // bullet prefab
    public GameObject swordPrefab;      // sword prefab
    public GameObject shotgunPrefab;    // shotgun prefab
    public GameObject machinegunPrefab; // machinegun prefab
    public static bool powerup1 = false;      // if shotgun powerup in use
    public static bool powerup2 = false;      // if Sword throw is in use
    public static bool powerup3 = false;      // if machinegun is in use
    private bool firelag = false;
    private bool rapidfire = false;
    public static int powerPoints = 1000;       // amount of weapon points
    public static int gamePoints = 0;           // The points collected throughout the game
    public static int bulletHitPoits = 0;       // Number of times bullet hit the enemy

    public Animator animator;           // for controlling animation

    private void Update()
    {
        if(!firelag)
        {
            StartCoroutine(Updater());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && powerPoints>=200 && !powerup1 && !powerup2 && !powerup3)
        {
            powerPoints = powerPoints - 200;
            Debug.Log("Weapon Points: "+ powerPoints);
            StartCoroutine(EnableShotgun());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && powerPoints >= 500 && !powerup1 && !powerup2 && !powerup3)
        {
            powerPoints = powerPoints - 500;
            Debug.Log("Weapon Points: " + powerPoints);
            StartCoroutine(EnableSwordthrow());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && powerPoints >= 800 && !powerup1 && !powerup2 && !powerup3)
        {
            powerPoints = powerPoints - 800;
            Debug.Log("Weapon Points: " + powerPoints);
            rapidfire = false;
            StartCoroutine(EnableMachinegun());
        }
    }
    IEnumerator Updater()
    {
        if (Input.GetButtonUp("Fire1") && !powerup1 && !firelag && !powerup3)
        {
            firelag = true;
            Shoot();
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(.6f);
            firelag = false;
        }
        else if (Input.GetButtonDown("Fire2") && !firelag && !powerup2)
        {
            firelag = true;
            Slash();
            yield return new WaitForSeconds(.4f);
            firelag = false;
        }
        else if(Input.GetButtonDown("Fire2") && !firelag && powerup2)
        {
            firelag = true;
            SwordThrow();
            yield return new WaitForSeconds(.4f);
            firelag = false;
        }
        else if (Input.GetButtonUp("Fire1") && powerup1 && !firelag)
        {
            firelag = true;
            animator.SetTrigger("Shoot");
            StartCoroutine(ShotgunShoot());
            yield return new WaitForSeconds(.6f);
            firelag = false;
        }
        else if (Input.GetButtonDown("Fire1") && powerup3 && !rapidfire)
        {
            rapidfire = true;
            while (powerup3 && rapidfire)
            {
                yield return new WaitForSeconds(.1f);
                MachinegunShoot();
                animator.SetTrigger("Shoot");
            }
        }
        else if (Input.GetButtonUp("Fire1") && rapidfire)
        {
            rapidfire = false;
        }
    }

    void Shoot ()
    {
        var shootobj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        shootobj.GetComponent<Bullet>().FireStart(1);
    }
    IEnumerator ShotgunShoot()
    {
        var shootobj = Instantiate(bulletPrefab, ShotgunFirePoint.position, ShotgunFirePoint.rotation);
        shootobj.GetComponent<Bullet>().FireStart(1);
        yield return new WaitForSeconds(.03f);
        var shootobj1 = Instantiate(bulletPrefab, ShotgunFirePoint.position, ShotgunFirePoint.rotation);
        shootobj1.GetComponent<Bullet>().FireStart(2);
        yield return new WaitForSeconds(.03f);
        var shootobj2 = Instantiate(bulletPrefab, ShotgunFirePoint.position, ShotgunFirePoint.rotation);
        shootobj2.GetComponent<Bullet>().FireStart(3);
    }
    void MachinegunShoot()
    {
        Vector3 temp;
        if (PlayerMovement.facingRight)
        {
            temp = new Vector3(0.1f, .01f, 0) + firePoint.position;
        }
        else
        {
            temp = new Vector3(-0.1f, 0.01f, 0) + firePoint.position;
        }
        var shootobj = Instantiate(bulletPrefab, temp, firePoint.rotation);
        shootobj.GetComponent<Bullet>().FireStart(1);
    }
    void SwordThrow()
    {
        var swordthrowobj = Instantiate(swordPrefab, firePoint.position, firePoint.rotation);
        swordthrowobj.GetComponent<Sword>().ThrowStart();
        Destroy(swordthrowobj, .5f);
    }
    void Slash()
    {
        var swordobj = Instantiate(swordPrefab, firePoint.position, firePoint.rotation);
        swordobj.GetComponent<Sword>().SlashStart();
        Destroy(swordobj, .35f);
    }
    IEnumerator EnableShotgun()
    {
        var shotgunobj = Instantiate(shotgunPrefab, ShotgunPoint.position, ShotgunPoint.rotation);
        powerup1 = true;
        yield return new WaitForSeconds(10);
        Destroy(shotgunobj);
        powerup1 = false;
    }
    IEnumerator EnableSwordthrow()
    {
        powerup2 = true;
        yield return new WaitForSeconds(10);
        powerup2 = false;
    }
    IEnumerator EnableMachinegun()
    {
        var machinegunobj = Instantiate(machinegunPrefab, firePoint.position, firePoint.rotation);
        powerup3 = true;
        yield return new WaitForSeconds(10);
        Destroy(machinegunobj);
        powerup3 = false;
    }
}
