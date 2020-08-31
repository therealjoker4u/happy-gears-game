using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedVideo : MonoBehaviour
{
    public GameObject extraGear;
    private int gotGears = 0;
    private int gearsLimit = 2;
    void Start()
    {
        gameObject.SetActive(false);
        extraGear.SetActive(false);
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
            GameObject extra = Instantiate(extraGear);
            extra.SetActive(true);
            extra.transform.position = new Vector3(-5f, GameController.globalGearHeight, -5f);
            extra.name = "rw"+Time.time;
            GameController.gearActions.Add(extra.name, extra.GetComponent<GearAction>());
            AndroidToast.show($"You got an extra gear successfully !!!");
            gotGears++;
            CloseMenu();

        }
        else
        {
            AndroidToast.show($"You can get only {gearsLimit} extra gears .");
            CloseMenu();
        }
    }
}
