using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPasswordUI : MonoBehaviour
{
    [SerializeField]
    SimpleMatchMaker simpleMatchMaker;
    [SerializeField]
    InputField roomNameField;
    [SerializeField]
    PhotonSimpleMatchMaker photonSimpleMatchMaker;

    public void OnClicked()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
            return;
        photonSimpleMatchMaker.JoinInternetMatch(roomNameField.text);
            //simpleMatchMaker.JoinTheMatch(RoomUI.SelectedRoom.MatchInfo, passwordField.text);
    }
}
