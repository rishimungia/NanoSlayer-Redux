using UnityEngine;

public class PowerUpHeal : MonoBehaviour
{
    [SerializeField]
    private float deltaY;
    [SerializeField]
    private GameObject healEffect;

    private Rigidbody2D _rigidBody;
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        
        initialPosition = _rigidBody.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, initialPosition.y + ((float)Mathf.Sin(Time.time) * deltaY));
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();
            
            if (playerHealth.Heal()) {
                Destroy(gameObject);

                GameObject healEffectObject = Instantiate(healEffect, transform.position, Quaternion.identity);
                Destroy(healEffectObject, 1.0f);
            }
        }
    }
}
