using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;
public class RewardedVideo : MonoBehaviour
{
    public GameObject extraGear;
    public static int gotGears = 0;
    private int gearsLimit = 5;
    public static string zoneId = "5f4fd90ad4366c0001e28aec";
    void Start()
    {
        gameObject.SetActive(false);
        extraGear.SetActive(false);
        gotGears = 0;
    }

    public void OpenMenu()
    {
        GameController.canDrag = false;
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        GameController.canDrag = true;
        gameObject.SetActive(false);
    }

    public void WatchVideo()
    {
        if(gotGears < gearsLimit)
        {
            AndroidToast.show("Please wait for a few seconds to load Ad.");

            Tapsell.RequestAd(RewardedVideo.zoneId, false,
                (TapsellAd ad) => {
                    Tapsell.ShowAd(ad);
                },
                (string notAvailable) =>
                {
                    AndroidToast.show("The service is not available, Try again");
                },
                (TapsellError error) => {
                    AndroidToast.show("Error on showing Ad , Try again");
                },
                (string networkError) => {
                    AndroidToast.show("You're not connected to internet !!!");
                },
                (TapsellAd expired) => {
                    AndroidToast.show("Rewarded video is expired");
                }

            );

            Tapsell.SetRewardListener((TapsellAdFinishedResult result) =>
                {
                    if(result.completed && result.rewarded)
                    {
                        GameObject extra = Instantiate(extraGear);
                        extra.SetActive(true);
                        extra.transform.position = new Vector3(-5f, GameController.globalGearHeight, -5f);
                        extra.name = "rw" + Time.time;
                        GameController.gearActions.Add(extra.name, extra.GetComponent<GearAction>());
                        AndroidToast.show($"You got an extra gear successfully .");
                        gotGears++;
                        CloseMenu();
                    }
                    else
                    {
                        AndroidToast.show($"You didn't watch all of Rewarded video , Try again");
                    }
                }
            );

        }
        else
        {
            AndroidToast.show($"You can get only {gearsLimit} extra gears .");
            CloseMenu();
        }
    }
}
