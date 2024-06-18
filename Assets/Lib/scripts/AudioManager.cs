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

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("non available sound");
        }
        else
        {
            musicScource.PlayOneShot(s.clip);
        }
    }

    public void PlayRandomSong()
    {
        string playSong = musicSounds[UnityEngine.Random.Range(0, musicSounds.Length)].name;
        Debug.Log(playSong);
        PlayMusic(playSong);
    }

    public IEnumerator PlayNext()
    {
        yield return new WaitUntil(() => !musicScource.isPlaying);
        PlayRandomSong();
        StartCoroutine(PlayNext());
    }
}
