using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPasswordUI : MonoBehaviour
{
    [SerializeField]
    SimpleMatchMaker simpleMatchMaker;
    [SerializeField]
    InputField passwordField;

    public void OnClicked()
    {
        if (string.IsNullOrEmpty(passwordField.text))
            return;
        else
            simpleMatchMaker.JoinTheMatch(RoomUI.SelectedRoom.MatchInfo, passwordField.text);
    }
}
