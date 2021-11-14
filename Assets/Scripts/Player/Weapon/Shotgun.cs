using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Transform target;
    public float speed=10;
    private Rigidbody2D _rigidbody;
    private bool rotate;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("ShotgunPoint").GetComponent<Transform>();
        rotate=PlayerMovement.facingRight;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate!=PlayerMovement.facingRight)
        {
            rotate = PlayerMovement.facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, target.position, speed * Time.deltaTime);
    }
}
