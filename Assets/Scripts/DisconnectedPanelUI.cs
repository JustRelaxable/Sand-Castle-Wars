using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectedPanelUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject disconnectedPanel;

    private bool joinedToRoom = false;
    public override void OnJoinedRoom()
    {
        joinedToRoom = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(joinedToRoom)
            disconnectedPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (joinedToRoom)
            disconnectedPanel.SetActive(true);
    }
}
