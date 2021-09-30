using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField]
    Text roomNameText;

    public static RoomUI SelectedRoom { get; private set; }
    public MatchInfoSnapshot MatchInfo { get; private set; }

    public void PrepareRoom(MatchInfoSnapshot matchInfo)
    {
        this.MatchInfo = matchInfo;
        roomNameText.text = matchInfo.name;
    }

    public void OnClicked()
    {
        if(SelectedRoom != null)
        {
            SelectedRoom.GetComponent<Button>().interactable = true;
        }
        SelectedRoom = this;
        GetComponent<Button>().interactable = false;
    }
}
