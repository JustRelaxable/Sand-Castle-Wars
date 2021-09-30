using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkRoomLister : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;
    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    private Transform roomListParent;
    // Use this for initialization
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        networkManager.StartMatchMaker();
        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        var myMatch = matches[0];
        networkManager.matchMaker.JoinMatch(myMatch.networkId, "", "", "", 0, 0,DataResponse);
        //foreach (var match in networkManager.matches)
        //{
        //    GameObject _roomlistItemGO = Instantiate(roomListItemPrefab);
        //    _roomlistItemGO.transform.SetParent(roomListParent);
        //    roomList.Add(_roomlistItemGO);
        //}
    }

    public void DataResponse(bool success, string extendedInfo, MatchInfo responseData)
    {
        networkManager.StartClient(responseData);
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }
}
