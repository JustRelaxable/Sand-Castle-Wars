using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GameCardHolderUI : MonoBehaviour
{
    public GameObject gameCardUIPrefab;
    public GameObject lastPlayedCard;
    public ClientGameManager clientGameManager;

    protected bool firstRound = true;

    private void Awake()
    {
        clientGameManager.OnGameStarted += ClientGameManager_OnGameStarted;
    }

    public void ClientGameManager_OnGameStarted()
    {
        var castleTurnControllers = FindObjectsOfType<CastleTurnController>();

        foreach (var item in castleTurnControllers)
        {
            item.OnTurnMine += MyCastleTurnController_OnTurnMine;
        }
    }

    public virtual void MyCastleTurnController_OnTurnMine(bool isTurnMine)
    {
        if (isTurnMine)
        {
            var gameCardUIs = GetComponentsInChildren<GameCardUI>();
            foreach (var gameCardUI in gameCardUIs)
            {
                gameCardUI.CloseCardBack();
            }
        }
        else
        {
            var gameCardUIs = GetComponentsInChildren<GameCardUI>();
            foreach (var gameCardUI in gameCardUIs)
            {
                gameCardUI.OpenCardBack();
            }
        }
    }

    public virtual void InstantiateCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card,false);
    }
    public virtual GameObject InstantiateCardAndReturn(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab,transform.parent);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card, false);
        return cardGO;
    }

    public GameObject InstantiateLastCardAndReturn(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, lastPlayedCard.transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card, true);
        return cardGO;
    }

    public void InstantiateLastCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, lastPlayedCard.transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card,true);
    }

    public virtual void InstantiateBonusCard(int gameCardID)
    {
        InstantiateCard(gameCardID);
    }

    public virtual void HandleCardsAbleToUse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GameCardUI>().HandleAbleToUse();
        }
    }

    public void ClearAllCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    public void ResetFirstPlay()
    {
        firstRound = true;
    }
}
