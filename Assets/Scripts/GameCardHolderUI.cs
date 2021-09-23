using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameCardHolderUI : MonoBehaviour
{
    public GameObject gameCardUIPrefab;
    public GameObject lastPlayedCard;
    public GameObject bonusCard;

    public void InstantiateCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card,false);
    }

    public void InstantiateLastCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, lastPlayedCard.transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card,true);
    }

    public void InstantiateBonusCard(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUIPrefab, bonusCard.transform);
        var card = CardManager.instance.GetCard(gameCardID);
        cardGO.GetComponent<GameCardUI>().PrepareCard(card, false);
        cardGO.GetComponent<GameCardUI>().cardSettings.GetComponent<CardSettingsUI>().DiscardButtonSetInteractable(false);
    }

    public void HandleCardsAbleToUse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<GameCardUI>().HandleAbleToUse();
        }
    }
}
