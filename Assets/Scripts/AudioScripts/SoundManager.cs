using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class SoundManager
{

    public enum Sound
    {
        PlayerSteps,
        LaserAttack,
        EnemyDie,
        Music,
        Ambience,
        Dashing,
        Jumping,
        PowerUp,
    }

    private static Dictionary<Sound, float> soundTimerDictionary; //Contiene i timer
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void PlaySound(Sound sound,Vector3 position)
    {

        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            soundGameObject.AddComponent<AudioSource>();
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }


    }

    public static void PlaySound( Sound sound)
    {

        if (CanPlaySound(sound))
        {

            if (oneShotGameObject == null)
            {
                oneShotGameObject= new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }

    
    }


    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Music] = -167f;
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.istanza.soundAudioClipsArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound" + sound + " not found ");
        return null;

    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.Music:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float MusicTimerMax = 167f; //Valore che determina quanto spesso il sound viene riprodotto

                    if(lastTimePlayed + MusicTimerMax < Time.time) //Se il tempo trascorso dall'ultima volta che il suono è stato riprodotto + il valore MusicTimerMax è minore di Time.time
                    {
                        soundTimerDictionary[sound] = Time.time;// si può far ripartire il sound
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
                //break;
        }
    }



}