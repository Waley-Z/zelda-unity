using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private static AudioSource underworldBGM;

    public enum Sound
    {
        EnemyDead,
        GameOver,
        HeartCollect,
        KeyCollect,
        KeyDrop,
        LowHp,
        RupeeCollect,
        Stairs,
        SwordFire,
        SwordSwipe,
        PlayerHurt,
        DoorOpen,
        OldManDoorOpen,
        OldManRoomEnter,
        Typewriter,
        Bomb,
        WeaponCollect,
        WallMasterAquamentus,
        Win,
    }

    private static Dictionary<Sound, float> soundTimerDic;
    private static Dictionary<Sound, float> soundMaxTimerDic;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            soundTimerDic = new Dictionary<Sound, float>();
            soundTimerDic[Sound.LowHp] = 0;
            soundTimerDic[Sound.Typewriter] = 0;
            soundTimerDic[Sound.SwordSwipe] = 0;
            soundMaxTimerDic = new Dictionary<Sound, float>();
            soundMaxTimerDic[Sound.LowHp] = .4f;
            soundMaxTimerDic[Sound.Typewriter] = .1f;
            soundMaxTimerDic[Sound.SwordSwipe] = .15f;

            underworldBGM = GameObject.Find("Underworld BGM").GetComponent<AudioSource>();
            if (underworldBGM == null)
            {
                Debug.LogError("bgm not found");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound(Sound sound, float volume = 1.0f)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.SetParent(Instance.transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            AudioClip clip = GetAudioClip(sound);
            audioSource.PlayOneShot(clip);

            Destroy(soundGameObject, clip.length);
        }
    }

    public static void pauseBGM()
    {
        underworldBGM.Pause();
    }
    public static void playBGM(bool restart=false)
    {
        if (restart)
        {
            underworldBGM.time = 0f;
        }
        underworldBGM.Play();
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.LowHp:
            case Sound.Typewriter:
            case Sound.SwordSwipe:
                if (soundTimerDic.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDic[sound];
                    float timerMax = soundMaxTimerDic[sound];
                    if (lastTimePlayed + timerMax < Time.time)
                    {
                        soundTimerDic[sound] = Time.time;
                        return true;
                    } else
                    {
                        return false;
                    }
                }    
                return true;

            default:
                return true;
        }
    }
    
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound not found");
        return null;
    }
}

