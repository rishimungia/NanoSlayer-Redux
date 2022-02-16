using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip bulletImpact, enemyDeath, jump, shoot, swordSlash, swordThrow;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        bulletImpact = Resources.Load<AudioClip>("bullet_impact");
        enemyDeath = Resources.Load<AudioClip>("enemy_death");
        jump = Resources.Load<AudioClip>("jump");
        shoot = Resources.Load<AudioClip>("shoot");
        swordSlash = Resources.Load<AudioClip>("sword_slash");
        swordThrow = Resources.Load<AudioClip>("sword_throw");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "bulletImpact":
                audioSrc.PlayOneShot(bulletImpact);
                break;
            case "enemyDeath":
                audioSrc.PlayOneShot(enemyDeath);
                break;
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "shoot":
                audioSrc.PlayOneShot(shoot);
                break;
            case "swordSlash":
                audioSrc.PlayOneShot(swordSlash);
                break;
            case "swordThrow":
                audioSrc.PlayOneShot(swordThrow);
                break;
        }
    }
}
