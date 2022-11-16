using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip validInputAudio;

    [SerializeField]
    AudioClip wrongInputSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip p_sound)
    {
        audioSource.clip = p_sound;
        audioSource.Play();
    }

    public void PlayValidInputSound()
    {
        PlaySound(validInputAudio);
    }

    public void PlayWrongInputSound()
    {
        PlaySound(wrongInputSound);
    }
}
