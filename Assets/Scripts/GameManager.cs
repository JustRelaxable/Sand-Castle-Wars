using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
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
    private void Start()
    {
        instance = this;
        spawnController = GetComponent<SpawnController>();
        teamController = GetComponent<TeamController>();
        clientGameManager = FindObjectOfType<ClientGameManager>();
    }

    public void OnPlayerConnect(NetworkConnection nc)
    {
        var castle = Instantiate(castlePrefab);
        var team = RequestTeam(castle);
        spawnController.ConfigureCastleTransform(team, castle);
        castle.GetComponent<PlayerCastle>().Team = team;

        NetworkServer.SpawnWithClientAuthority(castle, nc);

        UpdateCastleTeams();

        if (teamController.AreTeamsReady)
            StartGame();
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
            castle.RpcUpdateTeams(castle.Team);
        }
    }

    public void StartGame()
    {
        clientGameManager.RpcStartGame();
        PrepareCastleCards();
        AssignStartingPlayer();
    }

    private void AssignStartingPlayer()
    {
        var randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0)
        {
            teamController.blueCastle.GetComponent<CastleTurnController>().RpcNextTurn();
            currentTurn = Teams.Blue;
        }
        else
        {
            teamController.redCastle.GetComponent<CastleTurnController>().RpcNextTurn();
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
        teamController.blueCastle.GetComponent<PlayerCards>().RpcSetCardDeck(cardDeckToSend);

        for (int i = 0; i < cardDeckToSend.Length; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
            cardDeckToSend[i] = randomCardIndex;
        }
        teamController.redCastle.GetComponent<PlayerCards>().RpcSetCardDeck(cardDeckToSend);
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
            clientGameManager.RpcGetLastPlayedCard(cardID);

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
            clientGameManager.RpcGetLastPlayedCard(cardID);
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
            teamController.redCastle.GetComponent<CastleTurnController>().RpcNextTurn();
        }
           
        else if(currentTurn == Teams.Red)
        {
            teamController.redCastle.GetComponent<PlayerCards>().RpcTakeCard(GiveRandomCardIndex());
            currentTurn = Teams.Blue;
            HandleNextTurnResources(currentTurn);
            teamController.blueCastle.GetComponent<CastleTurnController>().RpcNextTurn();
        }
        
    }

    private void HandleNextTurnResources(Teams currentTurn)
    {
        switch (currentTurn)
        {
            case Teams.Blue:
                teamController.blueCastle.GetComponent<CastleStats>().HandleNextTurnResources();
                break;
            case Teams.Red:
                teamController.redCastle.GetComponent<CastleStats>().HandleNextTurnResources();
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
}
