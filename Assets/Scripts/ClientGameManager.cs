using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientGameManager : NetworkBehaviour
{
    private CastleStatsUI castleStatsUI;
    private GameCardHolderUI gameCardHolderUI;
    public event Action OnGameStarted;

    private void Start()
    {
        castleStatsUI = GameObject.FindObjectOfType<CastleStatsUI>();
        gameCardHolderUI = GameObject.FindObjectOfType<GameCardHolderUI>();
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        OnGameStarted?.Invoke();
        var castles = FindObjectsOfType<PlayerCastle>();
        foreach (var castle in castles)
        {
            castle.SetStaticCastleStat();
        }
        castleStatsUI.OnGameStarted();
    }

    [ClientRpc]
    public void RpcGetLastPlayedCard(int cardID)
    {
        for (int i = 0; i < gameCardHolderUI.lastPlayedCard.transform.childCount; i++)
        {
            Destroy(gameCardHolderUI.lastPlayedCard.transform.GetChild(i).gameObject);
        }
        gameCardHolderUI.InstantiateLastCard(cardID);
    }
}
