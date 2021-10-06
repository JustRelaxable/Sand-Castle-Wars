using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnglePlayerGameManager : MonoBehaviour
{
    BaseSinglePlayerBot singlePlayerBot;
    SpawnController spawnController;

    [SerializeField]
    GameObject castlePrefab;

    [SerializeField]
    CastleStatsUI castleStatsUI;

    [SerializeField]
    GameCardHolderUI gameCardHolderUI;

    [SerializeField]
    public GameObject adManager;

    [SerializeField]
    LastPlayedCardUI lastPlayedCardUI;


    CastleStats playerStats;
    CastleStats botStats;

    public AnimationCurve lastPlayedCurve;
    private Transform lastPlayedCardParentTransform;
    public GameObject nullPoint;
    private bool isLastCardArrived = false;
    private bool gameFinished = false;
    public GameObject cardDeck;
    public GameObject cardShowing;
    public GameObject cardBlueSpawnPoint;
    private void Awake()
    {
        singlePlayerBot = new SinglePlayerHardBot();
        spawnController = GetComponent<SpawnController>();
    }

    public void SetupScene()
    {
        GameCardBase.ChangeGameCardVariation(new GameCardSingle());
        adManager.gameObject.SetActive(true);
        
        

        var playerCastle = Instantiate(castlePrefab);
        spawnController.ConfigureCastleTransform(Teams.Red, playerCastle);
        playerCastle.GetComponent<PlayerCastle>().Team = Teams.Red;
        playerCastle.GetComponent<PlayerCastle>().UpdateTeamFlagMaterials(Teams.Red);
        playerCastle.GetComponent<PlayerCastle>().SetStaticCastleStatSingle();
        playerStats = playerCastle.GetComponent<CastleStats>();


        var botCastle = Instantiate(castlePrefab);
        spawnController.ConfigureCastleTransform(Teams.Blue, botCastle);
        botCastle.GetComponent<PlayerCastle>().Team = Teams.Blue;
        botCastle.GetComponent<PlayerCastle>().UpdateTeamFlagMaterials(Teams.Blue);
        botStats = botCastle.GetComponent<CastleStats>();

        gameCardHolderUI.ClientGameManager_OnGameStarted();
        castleStatsUI.OnGameStarted();
        PrepareCastleCards(playerCastle.GetComponent<PlayerCards>());

        adManager.GetComponent<RoundBasedAds>().ClientGameManager_OnGameStarted();

        playerStats.GetComponent<CastleTurnController>().InvokeOnTurnMine();
    }

    private void PrepareCastleCards(PlayerCards playerCards)
    {
        int[] cardDeckToSend = new int[8];
        for (int i = 0; i < cardDeckToSend.Length; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
            cardDeckToSend[i] = randomCardIndex;
            gameCardHolderUI.InstantiateCard(randomCardIndex);
        }
        

        for (int i = 0; i < cardDeckToSend.Length; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
            cardDeckToSend[i] = randomCardIndex;
        }

        singlePlayerBot.deck = cardDeckToSend.ToList();
    }

    public void PlayCard(int gameCardID,bool isDiscarded)
    {
        if (gameFinished)
            return;

        ClearLastPlayedCards();
       

        if (isDiscarded)
        {
            GameCardUI.selectedCard.OpenDiscarded();
        }
        else
        {
            CardManager.instance.GetCard(gameCardID).UseTheCard(playerStats, botStats);
        }
            
        GameCardUI.selectedCard.CloseCardSettings();
        var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.HandleNewCardTransform());
        StartCoroutine(MoveLastPlayedCard(GameCardUI.selectedCard.gameObject,cardPosToGo));
        GameCardUI.selectedCard.transform.parent = gameCardHolderUI.lastPlayedCard.transform;
        gameCardHolderUI.StartCoroutine(((GameCardHolder3DUI)gameCardHolderUI).TurnCardsBack());


        var gameCard = gameCardHolderUI.InstantiateCardAndReturn(UnityEngine.Random.Range(0, CardManager.instance.cards.Length));
        StartCoroutine(CardTakenFromDeck(gameCard));

        if (CheckIfGameFinished(playerStats, botStats))
            return;

        StartCoroutine(HandleBot());
    }

    private IEnumerator HandleBot()
    {
        botStats.HandleNextTurnResources();
        yield return new WaitForSeconds(5f);
        var botResponse = singlePlayerBot.HandleTurn(botStats, singlePlayerBot.deck.ToArray());
        singlePlayerBot.deck.Remove(CardManager.instance.GetIndex(botResponse.gameCard));
        var botCardID = UnityEngine.Random.Range(0, CardManager.instance.cards.Length);
        singlePlayerBot.deck.Add(botCardID);


        var botGameCard = gameCardHolderUI.InstantiateCardAndReturn(CardManager.instance.GetIndex(botResponse.gameCard));
        if (botResponse.isDiscarded)
        {
            botGameCard.GetComponent<GameCardUI>().OpenDiscarded();
        }
        else
        {
            botResponse.gameCard.UseTheCard(botStats, playerStats);
        }

        ClearLastPlayedCards();

            
        botGameCard.transform.position = cardBlueSpawnPoint.transform.position;

        var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.HandleNewCardTransform());
        StartCoroutine(MoveCardToTransform(botGameCard, cardPosToGo));
        botGameCard.transform.SetParent(lastPlayedCardUI.transform);
        //StartCoroutine(MoveCardToTransformAndChangeParent(botGameCard, gameCardHolderUI.lastPlayedCard.transform));

        if (CheckIfGameFinished(playerStats, botStats))
            yield break;

        playerStats.HandleNextTurnResources();
        gameCardHolderUI.HandleCardsAbleToUse();
        playerStats.GetComponent<CastleTurnController>().InvokeOnTurnMine();
    }
    private bool CheckIfGameFinished(CastleStats callerStats, CastleStats enemyStats)
    {
        if (callerStats.castleHeight >= 100 || enemyStats.castleHeight <= 0)
        {
            var turnIndicator = FindObjectOfType<TurnIndicatorUI>();
            turnIndicator.SetIndicatorText("You Win!");
            gameFinished = true;
            return true;
        }
        else if (callerStats.castleHeight <= 0 || enemyStats.castleHeight >= 100)
        {
            var turnIndicator = FindObjectOfType<TurnIndicatorUI>();
            turnIndicator.SetIndicatorText("You Lose!");
            gameFinished = true;
            return true;
        }
        return false;
    }
    public void SetLastPlayedCardParentTransform(Transform transform)
    {
        lastPlayedCardParentTransform = transform;
    }


    private void ClearLastPlayedCards()
    {
        //for (int i = 0; i < gameCardHolderUI.lastPlayedCard.transform.childCount; i++)
        //{
        //    Destroy(gameCardHolderUI.lastPlayedCard.transform.GetChild(i).gameObject);
        //}
    }

    private IEnumerator MoveCardToTransform(GameObject go, Transform lastTransform, float duration = 2f)
    {
        var currentPosition = go.transform.position;
        var time = 0f;
        float curve = 0f;
        while (time <= duration)
        {
            curve = lastPlayedCurve.Evaluate((time / duration));
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
        yield return MoveCardToTransform(go, cardShowing.transform, 1f);
        go.GetComponent<GameCard3DUI>().animator.SetTrigger("TurnBack");
        yield return MoveCardToTransformAndChangeParent(go, lastPlayedCardParentTransform);
    }
    private IEnumerator MoveLastPlayedCard(GameObject go,Vector3 posToGo)
    {
        yield return MoveCardToTransform(go, posToGo, 1f);
        isLastCardArrived = true;
    }
}
