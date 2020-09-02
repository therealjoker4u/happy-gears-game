using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TapsellSDK;

public class MainMenu : MonoBehaviour
{
    public Image musicBtn;
    public Sprite musicOn;
    public Sprite musicOff;
    
    void Start()
    {
        Tapsell.Initialize("lpbksjjdkabpfmgiqicifpmdskecjeqbfpqfjmqatckegqpckmeqtfdrrnkqledfsmsdok");
        if (PlayerPrefs.GetInt("music_on", 1) == 0)
        {
            musicBtn.sprite = musicOff;
        }
        Tapsell.RequestAd(RewardedVideo.zoneId, false, 
            (TapsellAd ad)=> {
                Tapsell.ShowAd(ad);
                AndroidToast.show("Ad is ready to show");
                print("Ad is ready to show");
            },
            (string notAvailable) =>
            {
                AndroidToast.show("service is not available");
                print("service is not available");
            },
            (TapsellError error) => { 
                AndroidToast.show("error on show ad");
                print("error on show ad");
                print(error.message);
            },
            (string networkError) => { 
                AndroidToast.show("You are not connected to internet for showing Rewarded video.");
                print("You are not connected to internet for showing Rewarded video.");
            },
            (TapsellAd expired) => { 
                AndroidToast.show("Rewarded video is expired");
                print("You are not connected to internet for showing Rewarded video.");
            }

        );
        Tapsell.SetRewardListener((TapsellAdFinishedResult result) =>
            {
                AndroidToast.show(result.completed && result.rewarded ? "You just got a gear" : "You did not finish rewarded video");
            }
        );
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
