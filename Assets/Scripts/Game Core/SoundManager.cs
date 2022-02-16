using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sounds {
        PlayerMove,
        PlayerShoot,
        PlayerDash,
        PlayerPowerDash,
        PlayerJump,
        PlayerHeal,

        GenericEnemyDeath,
        PlantDeath,
        JumperJump,
        CrabExplode,
        CrabActivate,

        BarrelExplode,
        BulletImpact
    }

    [SerializeField]
    private AudioClips[] audioClips;

    [System.Serializable]
    private class AudioClips {
        [SerializeField]
        private Sounds sound;
        [SerializeField]
        private AudioClip audioClip;
    } 

    public static PlaySound(Sounds sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot()
    }
}
