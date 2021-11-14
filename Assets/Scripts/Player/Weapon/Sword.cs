using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public int damage = 70;
    public float speed =10;

    public float knockbackX = 1.0f;
    public float knockbackY = 1.0f;

    private Transform target;
    private Rigidbody2D _rigidbody;
    private bool swordthrow = false;

    // Start is called before the first frame update
    public void SlashStart()
    {
        SoundManagerScript.PlaySound("swordSlash");
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.position = _rigidbody.position + new Vector2(0.3f, 0);
        animator = GetComponent<Animator>();
        animator.SetTrigger("Sword_Cut");
        target = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Transform>();
    }
    public void ThrowStart()
    {
        SoundManagerScript.PlaySound("swordThrow");
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Sword_Cut");
        target = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Transform>();
        swordthrow = true;
    }

    void Update()
    {
        Vector3 temp;
        if (!swordthrow) {
            if (PlayerMovement.facingRight) {
                temp = new Vector3(0.04f, 0, 0) + target.position;
            }
            else {
                temp = new Vector3(-0.04f, 0, 0) + target.position;
            }
            _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, temp, speed * Time.deltaTime);
        }
        else {
            _rigidbody.velocity = transform.right * 3f;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy !=null) {
            enemy.TakeDamage(damage);
        }

        BarrelExplode barrel = hitInfo.GetComponent<BarrelExplode>();
        if(barrel != null) {
            barrel.TakeDamage(1);
        }

        // sword knockback 
        if(hitInfo.tag == "Barrel" || hitInfo.tag == "Enemy") {
            Rigidbody2D prop = hitInfo.gameObject.GetComponent<Rigidbody2D>();
            if(PlayerMovement.facingRight)
                prop.AddForce(new Vector2(knockbackX, knockbackY), ForceMode2D.Impulse);
            else
                prop.AddForce(new Vector2(-knockbackX, knockbackY), ForceMode2D.Impulse);
        }
    }
}
