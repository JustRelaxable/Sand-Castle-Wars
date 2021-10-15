using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNetworkObject : MonoBehaviour
{
    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (photonView.IsMine)
            photonView.RPC("OnPlayerConnectRpc", RpcTarget.MasterClient,photonView.Owner);

        //OnPlayerConnectRpc();

        /*
        if (!isLocalPlayer)
            return;

        CmdOnPlayerConnect();
        */
    }
    [PunRPC]
    private void OnPlayerConnectRpc(Player player)
    {
        GameManager.instance.OnPlayerConnect(player);
    }
}
