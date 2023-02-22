using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] sfxSounds;
    public AudioSource sfxSource;
    public bool randomPitch = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    public void PlaySFX(string name)
    {
        Audio s = Array.Find(sfxSounds, x => x.soundName == name);

        if(s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            if (randomPitch)
            {
                sfxSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                sfxSource.volume = 0.15f;
            }

            if (!randomPitch)
            {
                sfxSource.volume = 0.5f;
            }
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
