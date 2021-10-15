using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListUIJoinButton : MonoBehaviour
{
    [SerializeField]
    SimpleMatchMaker simpleMatchMaker;

    [SerializeField]
    GameObject enterRoomPasswordPanel;

    [SerializeField]
    PhotonSimpleMatchMaker photonSimpleMatchMaker;

    public void OnClicked()
    {
        photonSimpleMatchMaker.JoinInternetMatch("test");
        /*
        if (RoomUI.SelectedRoom.IsPrivate)
            enterRoomPasswordPanel.SetActive(true);
        else
            simpleMatchMaker.JoinTheMatch(RoomUI.SelectedRoom.MatchInfo);
        */
    }
}
