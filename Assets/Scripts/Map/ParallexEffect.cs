using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    [SerializeField]
    private float effectStrength;
    [SerializeField]
    private bool isBackground;
    [SerializeField]
    private float propEffectClamp = 10.0f;
    [SerializeField]
    private bool repeatSprite = false;

    private GameObject cam;

    private float startpos;
    private float spriteLength;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        startpos = transform.position.x;

        if (repeatSprite)
            spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;    
    }

    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * effectStrength);

        if (!isBackground)
            transform.position = new Vector2(startpos + (dist / propEffectClamp), transform.position.y);
        else   
            transform.position = new Vector2(startpos + dist, transform.position.y);

        if (repeatSprite) {
            float camPos = (cam.transform.position.x * (1 - effectStrength));

            if (camPos > startpos + spriteLength)
                startpos += spriteLength;
            else if (camPos < startpos - spriteLength)
                startpos -= spriteLength;
        }
    }
}
