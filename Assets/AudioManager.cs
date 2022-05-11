using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sound1,sound2;

    public void ReproduceAudio1()
    {
        sound1.Play();
    }

    public void ReproduceAudio2()
    {
        sound2.Play();
    }
}
