﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject castlePrefab;

    private SpawnController spawnController;
    private TeamController teamController;
    private ClientGameManager clientGameManager;
    private Teams currentTurn = Teams.Null;
    private bool gameFinished = false;

    private object cardInTheProgress = new object();
    private NetworkInstanceId lastPlayedID;

    private int adsFinishedID = -1;

    private void Awake()
    {
    }

    private void Start()
    {
        instance = this;
        spawnController = GetComponent<SpawnController>();
        teamController = GetComponent<TeamController>();
        clientGameManager = FindObjectOfType<ClientGameManager>();
    }

    public void OnPlayerConnect(Player player)
    {      
        var castle = PhotonNetwork.Instantiate("PlayerCastle", Vector3.zero, Quaternion.identity);
        castle.GetComponent<PhotonView>().TransferOwnership(player);
        var team = RequestTeam(castle);
        spawnController.ConfigureCastleTransform(team, castle);
        castle.GetComponent<PlayerCastle>().Team = team;
        //NetworkServer.SpawnWithClientAuthority(castle, nc);

        UpdateCastleTeams();
        UpdateCastleTransforms();

        if (teamController.AreTeamsReady)
            StartGame();
    }

    private void UpdateCastleTransforms()
    {
        var castles = FindObjectsOfType<PlayerCastle>();
        foreach (var castle in castles)
        {
            castle.GetComponent<PhotonView>().RPC("UpdateTransformsRpc",RpcTarget.All, castle.gameObject.transform.position,castle.gameObject.transform.rotation.eulerAngles);
        }
    }

    public Teams RequestTeam(GameObject castle)
    {
        teamController.AssignToTeam(castle);
        return GetTeam(castle);
    }

    public Teams GetTeam(GameObject castle)
    {
        return teamController.GetTeamFromGameObject(castle);
    }

    public void UpdateCastleTeams()
    {
        var castles = GameObject.FindObjectsOfType<PlayerCastle>();

        foreach (var castle in castles)
        {
            //castle.UpdateTeamsRpc(castle.Team);
            castle.GetComponent<PhotonView>().RPC("UpdateTeamsRpc", RpcTarget.All, castle.Team);
        }
    }

    public void StartGame()
    {
        clientGameManager.GetComponent<PhotonView>().RPC("StartGameRpc",RpcTarget.All);
        PrepareCastleCards();
        AssignStartingPlayer();
    }

    private void AssignStartingPlayer()
    {
        var randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0)
        {
            teamController.blueCastle.GetComponent<PhotonView>().RPC("NextTurnRpc",RpcTarget.All);
            currentTurn = Teams.Blue;
        }
        else
        {
            teamController.redCastle.GetComponent<PhotonView>().RPC("NextTurnRpc", RpcTarget.All);
            currentTurn = Teams.Red;
        }      
    }

    private void PrepareCastleCards()
    {
        int[] cardDeckToSend = new int[8];
        for (int i = 0; i < cardDeckToSend.Length; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
            cardDeckToSend[i] = randomCardIndex;
        }
        teamController.blueCastle.GetComponent<PhotonView>().RPC("SetCardDeckRpc",RpcTarget.All,cardDeckToSend);

        for (int i = 0; i < cardDeckToSend.Length; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
            cardDeckToSend[i] = randomCardIndex;
        }
        teamController.redCastle.GetComponent<PhotonView>().RPC("SetCardDeckRpc", RpcTarget.All, cardDeckToSend);
    }

    public void UseCard(NetworkInstanceId id,int cardID)
    {

        lock (cardInTheProgress)
        {
            if (lastPlayedID == id)
                return;
            lastPlayedID = id;
            if (gameFinished)
                return;

            Teams callerTeam = teamController.GetTeamFromGameObject(NetworkServer.FindLocalObject(id));
            Teams enemyTeam = callerTeam == Teams.Blue ? Teams.Red : Teams.Blue;

            var callerStats = teamController.GetGameObjectFromTeam(callerTeam).GetComponent<CastleStats>();
            var enemyStats = teamController.GetGameObjectFromTeam(enemyTeam).GetComponent<CastleStats>();

            CardManager.instance.GetCard(cardID).UseTheCard(callerStats, enemyStats);
            clientGameManager.RpcGetLastPlayedCard(cardID,false);
            clientGameManager.RpcPlayCardAnimation(cardID, ((byte)currentTurn));

            if (CheckIfGameFinished(callerStats, enemyStats))
                return;

            NextTurn();
        }
    }

    public void DiscardCard(NetworkInstanceId id,int cardID)
    {
        lock (cardInTheProgress)
        {
            if (lastPlayedID == id)
                return;
            lastPlayedID = id;
            if (gameFinished)
                return;
            clientGameManager.RpcGetLastPlayedCard(cardID,true);
            NextTurn();
        }
    }

    private bool CheckIfGameFinished(CastleStats callerStats, CastleStats enemyStats)
    {
        if(callerStats.castleHeight >= 100 || enemyStats.castleHeight <= 0)
        {
            gameFinished = true;
            callerStats.GetComponent<PlayerCastle>().RpcGameFinished();
        }
        else if(callerStats.castleHeight<=0 || enemyStats.castleHeight >= 100)
        {
            gameFinished = true;
            enemyStats.GetComponent<PlayerCastle>().RpcGameFinished();
        }
        return gameFinished;
    }

    private void NextTurn()
    {
        
        if (currentTurn == Teams.Blue)
        {
            teamController.blueCastle.GetComponent<PlayerCards>().RpcTakeCard(GiveRandomCardIndex());        
            currentTurn = Teams.Red;
            HandleNextTurnResources(currentTurn);
            teamController.redCastle.GetComponent<CastleTurnController>().NextTurnRpc();
        }
           
        else if(currentTurn == Teams.Red)
        {
            teamController.redCastle.GetComponent<PlayerCards>().RpcTakeCard(GiveRandomCardIndex());
            currentTurn = Teams.Blue;
            HandleNextTurnResources(currentTurn);
            teamController.blueCastle.GetComponent<CastleTurnController>().NextTurnRpc();
        }
        
    }

    private void HandleNextTurnResources(Teams currentTurn)
    {
        switch (currentTurn)
        {
            case Teams.Blue:
                teamController.blueCastle.GetComponent<CastleStats>().RpcHandleNextTurnResources();
                break;
            case Teams.Red:
                teamController.redCastle.GetComponent<CastleStats>().RpcHandleNextTurnResources();
                break;
            default:
                break;
        }
    }

    public int GiveRandomCardIndex()
    {
        return UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
    }

    public void GetBonusCard(NetworkInstanceId id)
    {
        var x = NetworkServer.FindLocalObject(id);
        x.GetComponent<PlayerCards>().RpcTakeBonusCard(GiveRandomCardIndex());
    }

    public void AdsFinished(NetworkInstanceId id)
    {
        if(adsFinishedID == -1)
        {
            adsFinishedID = (int)id.Value;
        }
        else if(adsFinishedID != (int)id.Value)
        {
            adsFinishedID = -1;
            clientGameManager.RpcAdsFinished();
        }
    }
}
