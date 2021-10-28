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

    [SerializeField]
    Sprite hlfSprite;

    [SerializeField]
    Sprite fullSprite;

    private Image imge;
    public static RoomUI SelectedRoom { get; private set; }

    public string RoomName { get; private set; }

    private void Awake()
    {
        imge = GetComponent<Image>();
    }

    public void PrepareRoom(string roomName,int playerCount)
    {
        roomNameText.text = roomName + $" {playerCount}/2";
        RoomName = roomName;
        if (playerCount == 1)
            imge.sprite = hlfSprite;
        else
            imge.sprite = fullSprite;
    }

    public void OnClicked()
    {
        /*
        if(SelectedRoom != null)
        {
            SelectedRoom.GetComponent<Button>().interactable = true;
        }
        SelectedRoom = this;
        GetComponent<Button>().interactable = false;
        */
        PhotonSimpleMatchMaker.instance.JoinInternetMatch(RoomName);
    }
}
