﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ClientGameManager : NetworkBehaviour
{
    private CastleStatsUI castleStatsUI;
    private GameCardHolderUI gameCardHolderUI;
    public event Action OnGameStarted;
    public CardAnimationSpawner cardAnimationSpawner;
    public GameObject cardSpawnPoint;
    public GameObject cardDeck;
    public GameObject lastCardPos;
    public AnimationCurve lastPlayedCurve;
    
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
        //gameCardHolderUI.InstantiateLastCard(cardID);
        var myTurn = FindObjectsOfType<CastleTurnController>().Single(x => x.hasAuthority).myTurn;
        
        if (myTurn)
        {
            GameCardUI.selectedCard.CloseCardSettings();
            
            StartCoroutine(MoveCardToTransform(GameCardUI.selectedCard.gameObject, gameCardHolderUI.lastPlayedCard.transform));
            GameCardUI.selectedCard.transform.parent = gameCardHolderUI.lastPlayedCard.transform;
        }
        else
        {
            var gameCard = gameCardHolderUI.InstantiateLastCardAndReturn(cardID);
            gameCard.transform.position = cardSpawnPoint.transform.position;
            StartCoroutine(MoveCardToTransform(gameCard,gameCardHolderUI.lastPlayedCard.transform));
        }
    }

    [ClientRpc]
    public void RpcPlayCardAnimation(int cardID,byte team)
    {
        var animationPrefab = CardManager.instance.cards[cardID].animationPrefab;
        if (animationPrefab == null)
            return;
        cardAnimationSpawner.HandleCardAnimation(animationPrefab, (Teams)team);
    }

    public void TakeCardFromDeck(GameObject go)
    {
        go.transform.position = cardDeck.transform.position;
        StartCoroutine(MoveCardToTransformAndPutToCard(go, lastCardPos.transform));
    }

    private IEnumerator MoveCardToTransform(GameObject go,Transform lastTransform)
    {
        var currentPosition = go.transform.position;
        var time = 0f;
        float duration = 2f;
        float curve = 0f;
        while (time<=duration)
        {
            curve = lastPlayedCurve.Evaluate((time/duration));
            go.transform.position = Vector3.Lerp(currentPosition, lastTransform.position, curve);
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveCardToTransformAndPutToCard(GameObject go, Transform lastTransform)
    {
        yield return MoveCardToTransform(go, lastTransform);
        go.transform.parent = gameCardHolderUI.transform;
    }
}
