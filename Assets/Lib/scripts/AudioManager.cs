using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicScource, sfxSource;

    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("non available sound");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlayMusic()
    {

    }

    // IEnumerator NextSong()
    // {

    //     // for some reason the idea was to loop through music in order. I don't remember why.
    //     yield return new WaitUntil(() => !musicScource.isPlaying);
    //     string playSong = sfxSounds[UnityEngine.Random.Range(0, sfxSounds.Length)].name;
    //     Sound s = Array.Find(sfxSounds, x => x.name == playSong);
    //     musicScource.PlayOneShot(s.clip);
    // }
}
