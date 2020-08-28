using System;
using System.Collections;
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

    private void Start()
    {
        globalGearHeight = 0f;
        gearActions = new Dictionary<string, GearAction>();
        relationships = new List<Relationship>();
        isLocked = false;
        lockedGears = new List<string>();
        sourceRotationSpeed = 100f;
        canDrag = true;

        GameObject[] gears = GameObject.FindGameObjectsWithTag("Gear");

        foreach (GameObject gear in gears)
        {
            string gearName = gear.gameObject.name;

            gearActions.Add(gearName, gear.GetComponent<GearAction>());

        }
        UpdateActions();

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
                    lockedGears.Add(gearAction.gameObject.name);

                if (!lockedGears.Contains(gearName))
                    lockedGears.Add(gearName);
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
        lockedGears = new List<string>();
        bool allGearsAreRotating = true;

        foreach(string gearName in gearActions.Keys)
        {
            GearAction action = gearActions[gearName];

            if (GearCanRotate(gearName, out float rotationSpeed))
            {
                action.rotationSpeed = rotationSpeed;
            }
            else
            {
                action.rotationSpeed = 0;
                allGearsAreRotating = false;
            }

        }
        if (allGearsAreRotating && canDrag)
        {
            PlayerWon();
        }

    }


    public static void ConnectGears(string firstGearName, string secondGearName)
    {
        if(!isConnected(firstGearName , secondGearName))
        {
            relationships.Add(new Relationship() { first = firstGearName, second = secondGearName });
            //UpdateActions();
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
        sourceRotationSpeed *= 1.6f;

        print("You Won!!");
        UpdateActions();

    }

    private void Update()
    {
        UpdateActions();
    }

}
