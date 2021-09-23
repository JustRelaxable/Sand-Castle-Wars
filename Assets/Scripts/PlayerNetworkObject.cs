using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkObject : NetworkBehaviour
{
    private void Start()
    {
        if (!isLocalPlayer)
            return;

        CmdOnPlayerConnect();
    }

    [Command]
    private void CmdOnPlayerConnect()
    {
        GameManager.instance.OnPlayerConnect(connectionToClient);
    }
}
