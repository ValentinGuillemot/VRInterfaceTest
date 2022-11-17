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

    /// <summary>
    /// Play 2D sound to AudioSource (a single sound at the same time)
    /// </summary>
    /// <param name="p_sound">Clip to play</param>
    public void PlaySound(AudioClip p_sound)
    {
        audioSource.clip = p_sound;
        audioSource.Play();
    }

    /// <summary>
    /// Play stored 2D sound for valid inputs
    /// </summary>
    public void PlayValidInputSound()
    {
        PlaySound(validInputAudio);
    }

    /// <summary>
    /// Play stored 2D sound for invalid inputs
    /// </summary>
    public void PlayWrongInputSound()
    {
        PlaySound(wrongInputSound);
    }
}
