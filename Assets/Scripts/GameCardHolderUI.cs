using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GameCardHolderUI : MonoBehaviour
{
    public GameObject gameCardUIPrefab;
    public GameObject lastPlayedCard;
    public GameObject bonusCard;
    public ClientGameManager clientGameManager;

    private void Awake()
    {
        clientGameManager.OnGameStarted += ClientGameManager_OnGameStarted;
    }

    private void ClientGameManager_OnGameStarted()
    {
        var castleTurnControllers = FindObjectsOfType<CastleTurnController>();

        foreach (var item in castleTurnControllers)
        {
            item.OnTurnMine += MyCastleTurnController_OnTurnMine;
        }
    }

    private void MyCastleTurnController_OnTurnMine(bool isTurnMine)
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

    public void InstantiateCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card,false);
    }
    public GameObject InstantiateCardAndReturn(int gameCardID)
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

    public void InstantiateBonusCard(int gameCardID)
    {
        InstantiateCard(gameCardID);
        //Old behavior
        /*
        var cardGO = Instantiate(gameCardUIPrefab, bonusCard.transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card, false);
        cardGO.GetComponent<GameCardUI>().cardSettings.GetComponent<CardSettingsUI>().DiscardButtonSetInteractable(false);
        */
    }

    public void HandleCardsAbleToUse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GameCardUI>().HandleAbleToUse();
        }
    }
}
