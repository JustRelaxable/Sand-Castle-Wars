using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ClientGameManager : NetworkBehaviour
{
    [SerializeField]
    LastPlayedCardUI lastPlayedCardUI;

    private CastleStatsUI castleStatsUI;
    private GameCardHolderUI gameCardHolderUI;
    public event Action OnGameStarted;
    public CardAnimationSpawner cardAnimationSpawner;
    public GameObject cardRedSpawnPoint;
    public GameObject cardBlueSpawnPoint;
    public GameObject cardDeck;
    public GameObject lastCardPos;
    public GameObject cardShowing;
    public AnimationCurve lastPlayedCurve;
    private Transform lastPlayedCardParentTransform;
    public GameObject nullPoint;
    private bool isLastCardArrived = false;
    
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
    public void RpcGetLastPlayedCard(int cardID,bool isDiscarded)
    {
        ClearLastPlayedCards();
        //gameCardHolderUI.InstantiateLastCard(cardID);
        var myTurn = FindObjectsOfType<CastleTurnController>().Single(x => x.hasAuthority).myTurn;

        if (myTurn)
        {
            if (isDiscarded)
                GameCardUI.selectedCard.OpenDiscarded();
            GameCardUI.selectedCard.CloseCardSettings();
            var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.HandleNewCardTransform());
            StartCoroutine(MoveLastPlayedCard(GameCardUI.selectedCard.gameObject,cardPosToGo));
            GameCardUI.selectedCard.transform.parent = gameCardHolderUI.lastPlayedCard.transform;
            gameCardHolderUI.StartCoroutine(((GameCardHolder3DUI)gameCardHolderUI).TurnCardsBack());
        }
        else
        {
            var gameCard = gameCardHolderUI.InstantiateCardAndReturn(cardID);
            if (isDiscarded)
                gameCard.GetComponent<GameCardUI>().OpenDiscarded();

            var team = FindObjectsOfType<CastleStats>().Single(x => !x.hasAuthority).team;

            switch (team)
            {
                case Teams.Blue:
                    gameCard.transform.position = cardBlueSpawnPoint.transform.position;
                    break;
                case Teams.Red:
                    gameCard.transform.position = cardRedSpawnPoint.transform.position;
                    break;
                default:
                    break;
            }

            var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.HandleNewCardTransform());
            StartCoroutine(MoveCardToTransform(gameCard, cardPosToGo));
            gameCard.transform.SetParent(lastPlayedCardUI.transform);
        }
    }

    private void ClearLastPlayedCards()
    {
        //if(gameCardHolderUI.transform.childCount == 3)
        //{
        //    Destroy(gameCardHolderUI.lastPlayedCard.transform.GetChild(1).gameObject);
        //}
        //for (int i = 0; i < gameCardHolderUI.lastPlayedCard.transform.childCount; i++)
        //{
        //    Destroy(gameCardHolderUI.lastPlayedCard.transform.GetChild(i).gameObject);
        //}
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
        StartCoroutine(CardTakenFromDeck(go));
    }

    private IEnumerator MoveCardToTransform(GameObject go,Transform lastTransform,float duration = 2f)
    {
        var currentPosition = go.transform.position;
        var time = 0f;
        float curve = 0f;
        while (time<=duration)
        {
            curve = lastPlayedCurve.Evaluate((time/duration));
            go.transform.position = Vector3.Lerp(currentPosition, lastTransform.position, curve);
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveCardToTransform(GameObject go, Vector3 lastPosition, float duration = 2f)
    {
        var currentPosition = go.transform.position;
        var time = 0f;
        float curve = 0f;
        while (time <= duration)
        {
            curve = lastPlayedCurve.Evaluate((time / duration));
            go.transform.position = Vector3.Lerp(currentPosition, lastPosition, curve);
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator MoveCardToTransformAndPutToCard(GameObject go, Transform lastTransform)
    {
        yield return MoveCardToTransform(go, lastTransform);
        go.transform.parent = lastPlayedCardParentTransform;
    }
    private IEnumerator MoveCardToTransformAndChangeParent(GameObject go, Transform lastTransform)
    {
        yield return MoveCardToTransform(go, lastTransform);
        go.transform.parent = lastTransform;
    }

    private IEnumerator CardTakenFromDeck(GameObject go)
    {
        go.transform.position = nullPoint.transform.position;
        while (!isLastCardArrived)
        {
            yield return null;
        }
        isLastCardArrived = false;
        go.transform.position = cardDeck.transform.position;
        yield return MoveCardToTransform(go, cardShowing.transform,1f);
        go.GetComponent<GameCard3DUI>().animator.SetTrigger("TurnBack");
        yield return MoveCardToTransformAndChangeParent(go, lastPlayedCardParentTransform);
    }
    private IEnumerator MoveLastPlayedCard(GameObject go, Vector3 posToGo)
    {
        yield return MoveCardToTransform(go, posToGo, 1f);
        isLastCardArrived = true;
    }

    public void SetLastPlayedCardParentTransform(Transform transform)
    {
        lastPlayedCardParentTransform = transform;
    }

    [ClientRpc]
    public void RpcAdsFinished()
    {
        var playWaiter = FindObjectOfType<PlayWaiter>();
        StartCoroutine(playWaiter.WaitForPlay(2f));
        FindObjectOfType<RoundBasedAds>().adsWatchedOnBothPlayers = true;
    }
}
