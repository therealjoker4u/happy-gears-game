using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject musicButton;
    private Image musicBtnImage;
    public Sprite musicOn;
    public Sprite musicOff;
    
    void Start()
    {
        gameObject.SetActive(false);
        musicBtnImage = musicButton.GetComponent<Image>();
        if(PlayerPrefs.GetInt("music_on", 1) == 0)
        {
            musicBtnImage.sprite = musicOff;
        }

    }
    public void Open()
    {
        gameObject.SetActive(true);
        GameSounds.PlayOneShot("lip");
        GameController.canDrag = false;
    }
    public void Close()
    {
        gameObject.SetActive(false);
        GameSounds.PlayOneShot("lip");
        GameController.canDrag = true;
    }

    public void MusicSwitch()
    {
        if(PlayerPrefs.GetInt("music_on", 1) == 1)
        {
            MusicController.Pause();
            musicBtnImage.sprite = musicOff;
        }
        else
        {
            MusicController.Play();
            musicBtnImage.sprite = musicOn;
        }
    }
    public void PlayLipSound()
    {
        GameSounds.PlayOneShot("lip");
    }
}
