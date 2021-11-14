using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPond : MonoBehaviour
{
    public float vanishin = 3f;
    private float next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(next< Time.time)
        {
            Destroy (gameObject);
            next = Time.time + vanishin;
        }
    }
    void OnTriggerEnter2D(Collider2D Col)
    {
            PlayerHealth p = Col.GetComponent<PlayerHealth>();
            if(Col.tag=="Player")
        {
            p.TakeDamage(15);
            Destroy (gameObject);
        }
         
        
        
    }
}
