using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonSimpleMatchMaker : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform roomListParent;

    [SerializeField]
    GameObject roomUIInstance;
    public bool connectedToMasterServer = false;
    public void CreateInternetMatch(string roomName,bool isPrivateGame)
    {
        if (!connectedToMasterServer)
            return;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 2,IsVisible = isPrivateGame ? false:true});
    }

    public void JoinInternetMatch(string roomName)
    {
        if (!connectedToMasterServer)
            return;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LoadInternetMatches()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "ALL");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomListParent.childCount; i++)
        {
            Destroy(roomListParent.GetChild(i).gameObject);
        }
        foreach (var item in roomList)
        {
            if (!item.RemovedFromList)
            {
                var go = Instantiate(roomUIInstance, roomListParent);
                go.GetComponent<RoomUI>().PrepareRoom(item.Name);
            }
        }
    }

    public void ConnectToMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        connectedToMasterServer = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.InstantiateRoomObject("GameManager", Vector3.zero, Quaternion.identity);
    }

    public override void OnJoinedRoom()
    {
        FindObjectOfType<CameraAnimatorManager>().StartGameAnimation();
        transform.parent.gameObject.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
}
