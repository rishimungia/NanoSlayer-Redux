using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum PlayerSounds {
        PlayerMove,
        PlayerShoot,
        PlayerDash,
        PlayerPowerDash,
        PlayerJump,
        PlayerJumpLand,
        PlayerHeal,
    }

    public enum EnemySounds {
        GenericEnemyDeath,
        
        PlantGrow,
        PlantAttack,
        PlantDeath,
        
        JumperJump,
        
        CrabShoot,

        SpiderIgnite,
        SpiderExplode,
    }

    public enum FXSounds {
        BarrelExplode,
        BulletImpactEnemy,
        BulletImpact,
        Damage,
        LavaDamage,
        Teleport,
    }

    [SerializeField]
    private PlayerAudioClips[] playerAudioClips;
    [System.Serializable]
    private class PlayerAudioClips {
        public PlayerSounds sound;
        public AudioClip audioClip;
    }

    [SerializeField]
    private EnemyAudioClips[] enemyAudioClips;
    [System.Serializable]
    private class EnemyAudioClips {
        public EnemySounds sound;
        public AudioClip audioClip;
    }
    
    [SerializeField]
    private FXAudioClips[] fxAudioClips;
    [System.Serializable]
    private class FXAudioClips {
        public FXSounds sound;
        public AudioClip audioClip;
    }

    private static AudioSource _audioSource;

    public static SoundManager Instance;
    
    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        if(_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }

    // play sound with 2D audio source
    public static void PlaySound(PlayerSounds sound) {_audioSource.PlayOneShot(GetAudioClip(sound));}
    public static void PlaySound(EnemySounds sound) {_audioSource.PlayOneShot(GetAudioClip(sound));}
    public static void PlaySound(FXSounds sound) {_audioSource.PlayOneShot(GetAudioClip(sound));}

    // play sound at position (3D)
    public static void PlaySound(PlayerSounds sound, Vector2 position) {AudioSource.PlayClipAtPoint(GetAudioClip(sound), position);}
    public static void PlaySound(EnemySounds sound, Vector2 position) {AudioSource.PlayClipAtPoint(GetAudioClip(sound), position);}
    public static void PlaySound(FXSounds sound, Vector2 position) {AudioSource.PlayClipAtPoint(GetAudioClip(sound), position);}

    // for continious sound 
    public static void PlaySound(AudioSource audioSource, PlayerSounds sound, bool playToggle) {
        audioSource.clip = GetAudioClip(sound);
        if(!audioSource.isPlaying && playToggle) {
            audioSource.Play();
        }
        else if(!playToggle) {
            audioSource.Stop();
        }
    }
    public static void PlaySound(AudioSource audioSource, EnemySounds sound, bool playToggle) {
        audioSource.clip = GetAudioClip(sound);
        if(!audioSource.isPlaying && playToggle) {
            audioSource.Play();
        }
        else if(!playToggle) {
            audioSource.Stop();
        }
    }
    public static void PlaySound(AudioSource audioSource, FXSounds sound, bool playToggle) {
        audioSource.clip = GetAudioClip(sound);
        if(!audioSource.isPlaying && playToggle) {
            audioSource.Play();
        }
        else if(!playToggle) {
            audioSource.Stop();
        }
    }

    
    // get audio clip
    private static AudioClip GetAudioClip(PlayerSounds sound) {
        foreach (SoundManager.PlayerAudioClips audioClips in SoundManager.Instance.playerAudioClips) {
            if(audioClips.sound == sound) {
                return audioClips.audioClip;
            }
        }
        return null;
    }
    private static AudioClip GetAudioClip(EnemySounds sound) {
        foreach (SoundManager.EnemyAudioClips audioClips in SoundManager.Instance.enemyAudioClips) {
            if(audioClips.sound == sound) {
                return audioClips.audioClip;
            }
        }
        return null;
    }
    private static AudioClip GetAudioClip(FXSounds sound) {
        foreach (SoundManager.FXAudioClips audioClips in SoundManager.Instance.fxAudioClips) {
            if(audioClips.sound == sound) {
                return audioClips.audioClip;
            }
        }
        return null;
    }
}
