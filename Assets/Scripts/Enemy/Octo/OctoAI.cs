using UnityEngine;

public class OctoAI : MonoBehaviour
{
    public float speed;                 // speed of chaseing
    public float lineOfSite;            // green area border ( if entered will follow and shoot)
    public float shootingRange;         // distance to shoot from
    public float firerate = 3f;
    private float nextFireTime;
    public GameObject acidBall;         // acid to shoot
    public GameObject acidBallParent;   // place to shoot from
    private Transform player;

    private Vector2 movement;
    private Rigidbody2D rb; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position,transform.position);
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer < shootingRange && nextFireTime < Time.time)
        {
            Instantiate(acidBall , acidBallParent.transform.position,Quaternion.identity);
            nextFireTime = Time.time + firerate;
        }
    }
}