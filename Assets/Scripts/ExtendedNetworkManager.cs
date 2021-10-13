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
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //Called when a client disconnects from the server
        NetworkServer.DestroyPlayersForConnection(conn);
        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError) { Debug.Log("ServerDisconnected due to error: " + conn.lastError); }
        }
        Debug.Log("A client disconnected from the server: " + conn);
    }

    public override void OnStopClient()
    {
        //Called when client or server returns to main menu by manual
        base.OnStopClient();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }
}
