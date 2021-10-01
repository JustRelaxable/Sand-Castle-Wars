using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ExtendedNetworkManager : NetworkManager
{
    public static event Action<NetworkConnection> onClientDisconnected;

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        onClientDisconnected?.Invoke(conn);
        Debug.Log("tt");
    }

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
        Debug.Log("tt");
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("tt");
    }
}
