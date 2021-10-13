using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class SimpleMatchMaker : MonoBehaviour
{
    [SerializeField]
    Transform roomListParent;

    [SerializeField]
    GameObject roomUIInstance;

    public static MatchInfo currentMatchInfo;
    void Start()
    {
        NetworkManager.singleton.StartMatchMaker();
    }

    //call this method to request a match to be created on the server
    public void CreateInternetMatch(string matchName, CreateRoomUI createRoomUI,string matchPassword = "")
    {
        NetworkManager.singleton.StartMatchMaker();
        NetworkManager.singleton.matchMaker.CreateMatch(matchName, 2, true,matchPassword, "", "", 0, 0, OnInternetMatchCreate);
        createRoomUI.transform.parent.gameObject.SetActive(false);
    }

    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Create match succeeded");
            currentMatchInfo = matchInfo;
            FindObjectOfType<CameraAnimatorManager>().StartGameAnimation();
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }

    //call this method to find a match through the matchmaker
    public void GetInternetMatches()
    {
        NetworkManager.singleton.StartMatchMaker();
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", false, 0, 0, LoadInternetMatchList);
    }

    public void JoinTheMatch(MatchInfoSnapshot mathcInfoSnapshot,string matchPassword = "")
    {
        NetworkManager.singleton.matchMaker.JoinMatch(mathcInfoSnapshot.networkId, matchPassword, "", "", 0, 0, OnJoinInternetMatch);
    }

    //this method is called when a list of matches is returned
    private void LoadInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            for (int i = 0; i < roomListParent.childCount; i++)
            {
                Destroy(roomListParent.GetChild(i).gameObject);
            }
            foreach (var item in matches)
            {
                var go = Instantiate(roomUIInstance, roomListParent);
                go.GetComponent<RoomUI>().PrepareRoom(item);
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
        }
    }

    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Able to join a match");
            currentMatchInfo = matchInfo;
            FindObjectOfType<CameraAnimatorManager>().StartGameAnimation();
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }
}