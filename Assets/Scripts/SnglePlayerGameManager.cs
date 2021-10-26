using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    [SerializeField]
    CardAnimationSpawner cardAnimationSpawner;

    [SerializeField]
    GameObject showOffCard;
    private GameObject selectedShowOffCard;
    private Transform showOffCardParent;
    private float showOffCardParentLocalScale;
    private Button showOffCardOverButton;

    CastleStats playerStats;
    CastleStats botStats;

    public AnimationCurve lastPlayedCurve;
    private Transform lastPlayedCardParentTransform;
    public GameObject nullPoint;
    private bool isLastCardArrived = false;
    private bool gameFinished = false;
    public bool GameFinished { get => gameFinished; set { gameFinished = value; } }
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

        ConfigurePlayerCastle();
        ConfigureBotCastle();

        gameCardHolderUI.ClientGameManager_OnGameStarted();
        castleStatsUI.OnGameStarted();
        PrepareCastleCards();

        adManager.GetComponent<RoundBasedAds>().ClientGameManager_OnGameStarted();

        playerStats.GetComponent<CastleTurnController>().InvokeOnTurnMine();
    }

    private void ConfigurePlayerCastle()
    {
        var playerCastle = ConfigureAndGetCastleGameObject(Teams.Red);
        playerStats = playerCastle.GetComponent<CastleStats>();
        playerCastle.GetComponent<PlayerCastle>().SetStaticCastleStatSingle();
    }

    private void ConfigureBotCastle()
    {
        var botCastle = ConfigureAndGetCastleGameObject(Teams.Blue);
        botStats = botCastle.GetComponent<CastleStats>();
    }

    private GameObject ConfigureAndGetCastleGameObject(Teams team)
    {
        var castle = Instantiate(castlePrefab);
        spawnController.ConfigureCastleTransform(team, castle);
        AssignCastleTeam(castle.GetComponent<PlayerCastle>(), team);
        return castle;
    }

    private void AssignCastleTeam(PlayerCastle playerCastle,Teams team)
    {
        playerCastle.Team = team;
        playerCastle.UpdateTeamFlagMaterials(team);
    }

    private void PrepareCastleCards()
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
        ShowOffCardClose();

        if (isDiscarded)
        {
            GameCardUI.selectedCard.OpenDiscarded();
        }
        else
        {
            CardManager.instance.GetCard(gameCardID).UseTheCard(playerStats, botStats);
            cardAnimationSpawner.HandleCardAnimation(gameCardID, Teams.Red);
        }
        
        GameCardUI.selectedCard.CloseCardSettings();
        GameCardUI.selectedCard.DeactivateButton();
        var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.GetNewCardLocalPosition());
        StartCoroutine(MoveLastPlayedCard(GameCardUI.selectedCard.gameObject,cardPosToGo));
        GameCardUI.selectedCard.transform.parent = gameCardHolderUI.lastPlayedCard.transform;
        StartCoroutine(ChangeScaleOfGameObject(GameCardUI.selectedCard.gameObject, lastPlayedCardUI.transform.GetChild(0), 1));
        gameCardHolderUI.StartCoroutine(((GameCardHolder3DUI)gameCardHolderUI).TurnCardsBack());

       


        var gameCard = gameCardHolderUI.InstantiateCardAndReturn(UnityEngine.Random.Range(0, CardManager.instance.cards.Length),false);
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


        var botGameCard = gameCardHolderUI.InstantiateCardAndReturn(CardManager.instance.GetIndex(botResponse.gameCard),true);
        if (botResponse.isDiscarded)
        {
            botGameCard.GetComponent<GameCardUI>().OpenDiscarded();
        }
        else
        {
            botResponse.gameCard.UseTheCard(botStats, playerStats);
            cardAnimationSpawner.HandleCardAnimation(botResponse.gameCard.ID, Teams.Blue);
        }

        botGameCard.GetComponent<GameCardUI>().DeactivateButton();
        botGameCard.transform.position = cardBlueSpawnPoint.transform.position;

        var cardPosToGo = lastPlayedCardUI.transform.TransformPoint(lastPlayedCardUI.GetNewCardLocalPosition());
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
        //go.transform.parent = lastTransform;
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
        StartCoroutine(MoveCardToTransform(go, lastPlayedCardParentTransform));
        go.transform.parent = lastPlayedCardParentTransform;
        var tranformedScale = go.transform.localScale.x * lastPlayedCardParentTransform.parent.localScale.x;
        StartCoroutine(ChangeScaleOfGameObject(go,tranformedScale, 1));
        //yield return MoveCardToTransformAndChangeParent(go, lastPlayedCardParentTransform);
    }
    private IEnumerator MoveLastPlayedCard(GameObject go,Vector3 posToGo)
    {
        yield return MoveCardToTransform(go, posToGo, 1f);
        isLastCardArrived = true;
    }
    private IEnumerator ChangeScaleOfGameObject(GameObject go,Transform scaleTransform,float duration = 2)
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
    private IEnumerator ChangeScaleOfGameObject(GameObject go,float scale, float duration = 2)
    {
        var currentScale = go.transform.localScale;
        var time = 0f;
        float curve = 0f;
        while (time <= duration)
        {
            curve = lastPlayedCurve.Evaluate((time / duration));
            go.transform.localScale = Vector3.Lerp(currentScale, new Vector3(scale,scale,scale), curve);
            time += Time.deltaTime;
            yield return null;
        }
    }
    public void ShowOffCardOpen(GameObject card,Button button)
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
        showOffCardOverButton = button;

    }
    public void ShowOffCardClose()
    {
        CameraCanvasBlur.instance.CloseBlur();
        selectedShowOffCard.GetComponent<Animator>().SetTrigger("CloseCardOptions");
        StartCoroutine(MoveCardToTransform(selectedShowOffCard, showOffCardParent, 1));
        selectedShowOffCard.transform.parent = showOffCardParent;
        StartCoroutine(ChangeScaleOfGameObject(selectedShowOffCard, showOffCardParentLocalScale, 1));
        StartCoroutine(EnableOverButtonafterSeconds(showOffCardOverButton));
    }

    private IEnumerator EnableOverButtonafterSeconds(Button button, int seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        button.enabled = true;
    }
}
