using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private float effectStrength;
    [SerializeField]
    private bool repeatSprite = false;

    private float startpos;
    private float spriteLength;
    
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * effectStrength);

        transform.position = new Vector2(startpos + dist, transform.position.y);

        if (repeatSprite) {
            float camPos = (cam.transform.position.x * (1 - effectStrength));

            if (camPos > startpos + spriteLength)
                startpos += spriteLength;
            else if (camPos <= startpos + spriteLength)
                startpos -= spriteLength;
        }
    }
}
