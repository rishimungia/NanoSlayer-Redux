using UnityEngine;


public class BarrelExplode : MonoBehaviour
{
    public int hitPoints = 2;           // barrel hit points
    public int weaponPoints = 500;      // points given for powerup when distroyed
    public GameObject explodeEffect;    // death effect

    public void TakeDamage(int damagePoints)
    {
        hitPoints -= damagePoints;

        if(hitPoints == 1) {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
        }

        if(hitPoints <= 0) {
            Explode();
        }
    }

    void Explode()
    {
        //SoundManagerScript.PlaySound("explode");
        if(!Weapon.powerup1 && !Weapon.powerup2 && !Weapon.powerup3) {
            Weapon.powerPoints = Weapon.powerPoints + weaponPoints;
        }

        if(Weapon.powerPoints > 1000) {
            Weapon.powerPoints = 1000;
        }

        Destroy(gameObject);

        GameObject explodeEffectBig = Instantiate(explodeEffect, transform.position, Quaternion.identity, explodeEffect.transform.parent);
        explodeEffectBig.transform.localScale = new Vector2(1.5f, 1.5f);

        GameObject explodeEffectSmall = Instantiate(explodeEffect, transform.position + new Vector3(-3f, 3f, 0f), Quaternion.identity);

        Destroy(explodeEffectBig, 0.4f);
        Destroy(explodeEffectSmall, 0.5f);
    }
    
    private void OnDrawGizmosSelected()
    {

    }
}
