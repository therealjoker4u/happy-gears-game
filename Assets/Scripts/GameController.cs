using System.Collections.Generic;
using UnityEngine;

public struct Relationship
{
    public string first;
    public string second;
}

enum GearDirection
{
    Static,
    Clock,
    PadClock,
}

public class GameController : MonoBehaviour
{
    public static float globalGearHeight;
    public static Dictionary<string,GearAction> gearActions;
    public static List<Relationship> relationships;
    public static bool isLocked;
    public static List<string> lockedGears ;
    public static float sourceRotationSpeed;
    public static bool canDrag;
    public Material _unexpectedMaterial;
    public static Material unexpectedMaterial ;
    public GameObject _gearBar;
    public static GameObject gearBar;

    public static Vector3 screenBounds;
    public static float boundOffset = 0.25f;

    public void test()
    {
        print("This is test for you");
    }

    private void Start()
    {
        globalGearHeight = 0f;
        gearActions = new Dictionary<string, GearAction>();
        relationships = new List<Relationship>();
        isLocked = false;
        lockedGears = new List<string>();
        sourceRotationSpeed = 120f;
        canDrag = true;

        if (gearBar == null)
        {
            gearBar = _gearBar;
        }

        GameObject[] gears = GameObject.FindGameObjectsWithTag("Gear");

        foreach (GameObject gear in gears)
        {
            string gearName = gear.gameObject.name;

            gearActions.Add(gearName, gear.GetComponent<GearAction>());

            /*GameObject bar = Instantiate(gearBar);
            bar.SetActive(true);
            bar.transform.position = gear.transform.position;
            bar.transform.SetParent(gear.transform);*/

        }
        UpdateActions();

        if(unexpectedMaterial == null)
        {
            unexpectedMaterial = _unexpectedMaterial;
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.y));
        screenBounds.x -= boundOffset;
        screenBounds.z -= boundOffset;
    }


    public static bool isConnected(string firstGearName, string secondGearName)
    {
        for (int i = 0; i < relationships.Count; i++)
        {
            if (relationships[i].first == firstGearName && relationships[i].second == secondGearName)
            {
                return true;
            }
        }
        return false;
    }

    public static bool isConnected(string firstGearName, string secondGearName, out int index)
    {
        index = -1;
        for(int i = 0; i < relationships.Count; i++)
        {
            if (relationships[i].first == firstGearName && relationships[i].second == secondGearName)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    public static List<string> GetRelationships(string gearName)
    {
        List<string> returnable = new List<string>();

        foreach (Relationship relationship in relationships)
        {
            if(relationship.first == gearName)
            {
                returnable.Add(relationship.second);
            }

        }

        return returnable;
    }

    public static List<GearAction> GetRelatedActions(string gearName)
    {
        List<GearAction> actions = new List<GearAction>();

        List<string> rels = GetRelationships(gearName);

        foreach(string related in rels)
        {
            actions.Add(gearActions[related]);
        }

        return actions;
    }

    public static bool GearCanRotate(string gearName , out float _rotationSpeed)
    {
        List<GearAction> actions = GetRelatedActions(gearName);
        GearDirection previousDir = GearDirection.Static;
        float rotationSpeed = 0;

        foreach(GearAction gearAction in actions)
        {
            GearDirection currentDir = GearDirection.Static;

            if (gearAction.rotationSpeed > 0)
                currentDir = GearDirection.Clock;
            else if (gearAction.rotationSpeed < 0)
                currentDir = GearDirection.PadClock;

            if(previousDir != GearDirection.Static && currentDir != GearDirection.Static && currentDir != previousDir)
            {

                if(!lockedGears.Contains(gearAction.gameObject.name))
                {
                    lockedGears.Add(gearAction.gameObject.name);
                }

                if (!lockedGears.Contains(gearName))
                {
                    lockedGears.Add(gearName);
                }
            }
            else
            {
                if(currentDir != GearDirection.Static)
                {
                    previousDir = currentDir;
                }

                if(gearAction.rotationSpeed != 0)
                {
                    rotationSpeed = gearAction.rotationSpeed * -1;
                }

            }

        }
        if (gearActions[gearName].isSource)
        {
            rotationSpeed = sourceRotationSpeed;
        }

        _rotationSpeed = rotationSpeed;

        return (lockedGears.Count < 1 && (previousDir != GearDirection.Static || gearActions[gearName].isSource) );

    }

    public static void UpdateActions()
    {
        
        bool allGearsAreRotating = true;

        foreach(string gearName in gearActions.Keys)
        {
            GearAction action = gearActions[gearName];
            lockedGears = new List<string>();

            if (GearCanRotate(gearName, out float rotationSpeed))
            {
                action.rotationSpeed = rotationSpeed;

            }
            else
            {
                action.rotationSpeed = 0;
                if(action.isTarget)
                    allGearsAreRotating = false;
            }

            if (lockedGears.Contains(gearName) && !action.isLocked)
            {
                allGearsAreRotating = false;
                action.isLocked = true;
                action.ChangeMaterialToUnexpected();
            }
            else if(!lockedGears.Contains(gearName) && action.isLocked)
            {
                allGearsAreRotating = false;
                action.isLocked = false;
                action.ChangeMaterialToNormal();
            }


        }

        if(lockedGears.Count > 0)
        {
            foreach (string gearName in gearActions.Keys)
            {
                GearAction action = gearActions[gearName];
                action.rotationSpeed = 0;
            }
        }else if(allGearsAreRotating && canDrag)
        {
            PlayerWon();
        }

    }


    public static void ConnectGears(string firstGearName, string secondGearName)
    {
        if(!isConnected(firstGearName , secondGearName))
        {
            relationships.Add(new Relationship() { first = firstGearName, second = secondGearName });
        }
    }

    public static void DisconnectGears(string firstGearName, string secondGearName)
    {
        if (isConnected(firstGearName, secondGearName, out int index))
        {

            relationships.RemoveAt(index);

            foreach(string gearName in gearActions.Keys)
            {
                gearActions[gearName].rotationSpeed = 0;

            }
            UpdateActions();
        }
    }

    public static void PlayerWon()
    {
        canDrag = false;
        int stars = 3 - RewardedVideo.gotGears;
        if (stars < 1)
            stars = 0;
        GameEnd.onWin(stars);

    }


    private void Update()
    {
        UpdateActions();
    }

}
