using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamController : MonoBehaviour
{
    public GameObject redCastle;
    public GameObject blueCastle;

    public bool AreTeamsReady
    {
        get
        {
            if (redCastle != null && blueCastle != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private set { }
    }

    public void AssignToTeam(GameObject castle)
    {
        if (redCastle == null && blueCastle == null)
        {
            var randNum = Random.Range(0, 2);
            switch (randNum)
            {
                case 0:
                    blueCastle = castle;
                    break;
                case 1:
                    redCastle = castle;
                    break;
                default:
                    break;
            }
        }
        else if (redCastle == null)
        {
            redCastle = castle;
        }
        else if (blueCastle == null)
        {
            blueCastle = castle;
        }
    }

    public Teams GetTeamFromGameObject(GameObject castle)
    {
        if(castle == redCastle)
        {
            return Teams.Red;
        }
        else if (castle == blueCastle)
        {
            return Teams.Blue;
        }
        else
        {
            return Teams.Null;
        }
    }

    public GameObject GetGameObjectFromTeam(Teams team)
    {
        if(team == Teams.Blue)
        {
            return blueCastle;
        }
        else if(team == Teams.Red)
        {
            return redCastle;
        }
        else
        {
            return null;
        }
    }
}


public enum Teams
{
    Blue,Test, Red,Null
}
