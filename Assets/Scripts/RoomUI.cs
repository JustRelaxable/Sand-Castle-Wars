using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField]
    Text roomNameText;

    [SerializeField]
    Image privateRoomImage;

    public static RoomUI SelectedRoom { get; private set; }

    public string RoomName { get; private set; }

    public void PrepareRoom(string roomName)
    {
        roomNameText.text = roomName;
        RoomName = roomName;
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
