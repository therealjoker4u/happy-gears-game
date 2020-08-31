using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image musicBtn;
    public Sprite musicOn;
    public Sprite musicOff;
    
    void Start()
    {
        if(PlayerPrefs.GetInt("music_on", 1) == 0)
        {
            musicBtn.sprite = musicOff;
        }
    }

    public void Play()
    {
        GameSounds.PlayOneShot("lip");
        SceneManager.LoadScene("Levels Menu");
    }
    public void SwitchMusic()
    {
        GameSounds.PlayOneShot("lip");
        if (PlayerPrefs.GetInt("music_on", 1) == 1)
        {
            musicBtn.sprite = musicOff;
            MusicController.Pause();
        }
        else
        {
            musicBtn.sprite = musicOn;
            MusicController.Play();
        }
    }


}
