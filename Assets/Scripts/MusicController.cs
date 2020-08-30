using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public static AudioSource audioSource;
    public static AudioClip successClip;

    public static AudioSource gameActionsSource;
    void Start()
    {
        if(gameActionsSource == null)
        {
            GameObject gameActionsSounds = new GameObject("Game Actions Sounds");
            DontDestroyOnLoad(gameActionsSounds);

            gameActionsSounds.AddComponent<AudioSource>();
            gameActionsSource = gameActionsSounds.GetComponent<AudioSource>();
            gameActionsSource.loop = false;

        }

        if(audioSource == null)
        {
            GameObject musicObj = new GameObject("Music Source");
            DontDestroyOnLoad(musicObj);
            musicObj.AddComponent<AudioSource>();

            audioSource = musicObj.GetComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Sounds/music-main");
            audioSource.loop = true;
            if (PlayerPrefs.GetInt("music_on", 1) == 1)
            {
                Play();
            }
        }

        if(successClip == null)
        {
            successClip = Resources.Load<AudioClip>("Sounds/success");
        }
    }

    public static void Play()
    {
        PlayerPrefs.SetInt("music_on", 1);
        audioSource.Play();
    }

    public static void Pause()
    {
        PlayerPrefs.SetInt("music_on", 0);
        audioSource.Pause();
    }

    public static void SwitchPlayingMusic()
    {
        if(PlayerPrefs.GetInt("music_on", 1) == 1)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }

    public static void PlayWinSound()
    {
        gameActionsSource.clip = successClip;
        gameActionsSource.Play();
    }
}
