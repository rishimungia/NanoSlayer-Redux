using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBall : MonoBehaviour
{   
    GameObject target; // the players
    public float speed; // speed of acid 
    Rigidbody2D bulletRB; 
    Collider2D Col;
    
    [SerializeField]
    GameObject acid_pond;
    
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 movDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(movDir.x,movDir.y);
        Destroy(this.gameObject,2);
    }
    
    void OnTriggerEnter2D(Collider2D Col)
    {
        PlayerHealth p = Col.GetComponent<PlayerHealth>();
        if(p != null) {
            p.TakeDamage(5);
            Destroy (gameObject);
        }
         else if(Col.tag=="GROUND")
        {
            Transform grnd = GameObject.FindGameObjectWithTag("GROUND").transform;
            makepond();
            
        }
    }

    void makepond()
    {
        Destroy (gameObject); 
        Instantiate(acid_pond,transform.position,Quaternion.identity);
    }
    
}