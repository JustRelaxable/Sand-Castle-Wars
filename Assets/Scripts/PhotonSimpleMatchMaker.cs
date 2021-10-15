using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonSimpleMatchMaker : MonoBehaviourPunCallbacks
{
    public bool connectedToMasterServer = false;
    public void CreateInternetMatch(string roomName)
    {
        if (!connectedToMasterServer)
            return;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 2 });
    }

    public void JoinInternetMatch(string roomName)
    {
        if (!connectedToMasterServer)
            return;
        PhotonNetwork.JoinRoom("test");
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
}
