using System.Collections;
using UnityEngine;


public class BarrelExplode : MonoBehaviour
{
    [SerializeField]
    private float explodeTimer = 1.0f;               // barrel explode delay
    [SerializeField]
    private GameObject explodeEffect;                // exploding effect

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        StartCoroutine("Explode");
    }

    IEnumerator Explode()
    {
        animator.SetBool("Ignited", true);

        yield return new WaitForSeconds(explodeTimer);
        
        SoundManagerScript.PlaySound("explode");
        Destroy(gameObject);

        CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
        GameObject explodeEffectBig = Instantiate(explodeEffect, transform.position, Quaternion.identity, explodeEffect.transform.parent);
        explodeEffectBig.transform.localScale = new Vector2(1.5f, 1.5f);

        GameObject explodeEffectSmall = Instantiate(explodeEffect, transform.position + new Vector3(-0.25f, 0.25f, 0f), Quaternion.identity);

        Destroy(explodeEffectBig, 0.4f);
        Destroy(explodeEffectSmall, 0.5f);
    }
    
}
