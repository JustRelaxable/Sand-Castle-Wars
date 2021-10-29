using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;
using UnityEngine.UI;

public class ClientGameManager : MonoBehaviourPun
{
    [SerializeField]
    LastPlayedCardUI lastPlayedCardUI;

    

    [SerializeField]
    private CastleStatsUI castleStatsUI;
    [SerializeField]
    private GameCardHolderUI gameCardHolderUI;

    [SerializeField]
    GameObject showOffCard;
    private GameObject selectedShowOffCard;
    private Transform showOffCardParent;
    private float showOffCardParentLocalScale;
    private Button showOffCardOverButton;

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

    [PunRPC]
    public void StartGameRpc()
    {
        OnGameStarted?.Invoke();
        var castles = FindObjectsOfType<PlayerCastle>();
        foreach (var castle in castles)
        {
            castle.SetStaticCastleStat();
        }
        castleStatsUI.OnGameStarted();
    }
    [PunRPC]
    public void GetLastPlayedCardRpc(int cardID,bool isDiscarded)
    {
        //ClearLastPlayedCards();
        //gameCardHolderUI.InstantiateLastCard(cardID);
        var myTurn = FindObjectsOfType<CastleTurnController>().Single(x => x.photonView.IsMine).myTurn;

        if (myTurn)
        {
            ShowOffCardClose(true);
            if (isDiscarded)
                GameCardUI.selectedCard.OpenDiscarded();
            GameCardUI.selectedCard.CloseCardSettings();
            GameCardUI.selectedCard.DeactivateButton();
            var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.GetNewCardLocalPosition());
            StartCoroutine(MoveLastPlayedCard(GameCardUI.selectedCard.gameObject, cardPosToGo));
            GameCardUI.selectedCard.transform.parent = gameCardHolderUI.lastPlayedCard.transform;
            StartCoroutine(ChangeScaleOfGameObject(GameCardUI.selectedCard.gameObject, lastPlayedCardUI.transform.GetChild(0), 1));
            gameCardHolderUI.DeactivateOvers();
            gameCardHolderUI.StartCoroutine(((GameCardHolder3DUI)gameCardHolderUI).TurnCardsBack());
        }
        else
        {
            var gameCard = gameCardHolderUI.InstantiateCardAndReturn(cardID,true);
            if (isDiscarded)
                gameCard.GetComponent<GameCardUI>().OpenDiscarded();

            gameCard.GetComponent<GameCardUI>().DeactivateButton();
            var team = FindObjectsOfType<CastleStats>().Single(x => !x.photonView.IsMine).team;

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

            var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.GetNewCardLocalPosition());
            StartCoroutine(MoveCardToTransform(gameCard, cardPosToGo));
            gameCardHolderUI.ActivateOvers();
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
    [PunRPC]
    public void PlayCardAnimationRpc(int cardID,byte team)
    {
        PlayCardAnimation(cardID, team);
    }

    public void PlayCardAnimation(int cardID, byte team)
    {
        cardAnimationSpawner.HandleCardAnimation(cardID, (Teams)team);
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
        go.GetComponent<GameCardUI>().DeactivateButton();
        go.transform.position = nullPoint.transform.position;
        while (!isLastCardArrived)
        {
            yield return null;
        }
        isLastCardArrived = false;
        go.transform.position = cardDeck.transform.position;
        yield return MoveCardToTransform(go, cardShowing.transform, 1f);
        go.GetComponent<GameCard3DUI>().animator.SetTrigger("TurnBack");
        StartCoroutine(MoveCardToTransform(go, lastPlayedCardParentTransform));
        go.transform.parent = lastPlayedCardParentTransform;
        var tranformedScale = go.transform.localScale.x * lastPlayedCardParentTransform.parent.localScale.x;
        StartCoroutine(ChangeScaleOfGameObject(go, tranformedScale, 1));
        StartCoroutine(EnableOverButtonafterSeconds(go.GetComponent<GameCardUI>().overButton, 1));
    }
    private IEnumerator MoveLastPlayedCard(GameObject go, Vector3 posToGo)
    {
        yield return MoveCardToTransform(go, posToGo, 1f);
        isLastCardArrived = true;
    }
    private IEnumerator ChangeScaleOfGameObject(GameObject go, Transform scaleTransform, float duration = 2)
    {
        var currentScale = go.transform.localScale;
        var time = 0f;
        float curve = 0f;
        while (time <= duration)
        {
            curve = lastPlayedCurve.Evaluate((time / duration));
            go.transform.localScale = Vector3.Lerp(currentScale, scaleTransform.localScale, curve);
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ChangeScaleOfGameObject(GameObject go, float scale, float duration = 2)
    {
        var currentScale = go.transform.localScale;
        var time = 0f;
        float curve = 0f;
        while (time <= duration)
        {
            curve = lastPlayedCurve.Evaluate((time / duration));
            go.transform.localScale = Vector3.Lerp(currentScale, new Vector3(scale, scale, scale), curve);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void SetLastPlayedCardParentTransform(Transform transform)
    {
        lastPlayedCardParentTransform = transform;
    }

    [PunRPC]
    public void AdsFinishedRpc()
    {
        var playWaiter = FindObjectOfType<PlayWaiter>();
        StartCoroutine(playWaiter.WaitForPlay(2f));
        FindObjectOfType<RoundBasedAds>().adsWatchedOnBothPlayers = true;
    }

    public void ShowOffCardOpen(GameObject card,Button overButton)
    {
        SetLastPlayedCardParentTransform(card.transform.parent);
        CameraCanvasBlur.instance.OpenBlur();
        selectedShowOffCard = card;
        showOffCardParent = card.transform.parent;
        showOffCardParentLocalScale = card.transform.localScale.x;
        StartCoroutine(MoveCardToTransform(card, showOffCard.transform, 1));
        card.transform.parent = showOffCard.transform;
        var showOffCardChild = showOffCard.transform.GetChild(0);
        StartCoroutine(ChangeScaleOfGameObject(card, showOffCardChild, 1));
        showOffCardOverButton = overButton;
    }
    public void ShowOffCardClose(bool isCardUsed)
    {
        CameraCanvasBlur.instance.CloseBlur();
        selectedShowOffCard.GetComponent<Animator>().SetTrigger("CloseCardOptions");
        StartCoroutine(MoveCardToTransform(selectedShowOffCard, showOffCardParent, 1));
        selectedShowOffCard.transform.parent = showOffCardParent;
        StartCoroutine(ChangeScaleOfGameObject(selectedShowOffCard, showOffCardParentLocalScale, 1));
        if(!isCardUsed)
            StartCoroutine(EnableOverButtonafterSeconds(showOffCardOverButton));
    }

    private IEnumerator EnableOverButtonafterSeconds(Button button,int seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        button.enabled = true;
    }
}
