using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardHolder3DUI : GameCardHolderUI
{
    public GameObject gameCardUI3D;
    bool firstRound = true;
    byte firstRoundCount = 0;
    public override void InstantiateCard(int gameCardID)
    {
        var gameCard3DUI = Instantiate(gameCardUIPrefab, transform);
        var card = CardManager.instance.GetCard(gameCardID);
        var gameCardUI = gameCard3DUI.GetComponentInChildren<GameCardUI>();
        gameCardUI.PrepareCard(card, false);
    }

    public override GameObject InstantiateCardAndReturn(int gameCardID)
    {
        var cardGO = Instantiate(gameCardUI3D, transform.parent);
        var card = CardManager.instance.GetCard(gameCardID);
        var gameCardUI = cardGO.GetComponent<GameCardUI>();
        gameCardUI.PrepareCard(card, false);
        return cardGO;
    }

    public override void InstantiateBonusCard(int gameCardID)
    {
        var cardGO = InstantiateCardAndReturn(gameCardID);

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).childCount == 0)
            {
                cardGO.transform.position = transform.GetChild(i).transform.position;
                cardGO.transform.parent = transform.GetChild(i).transform;
            }
        }
    }

    public override void HandleCardsAbleToUse()
    {
        var compenents = GetComponentsInChildren<GameCardUI>();
        for (int i = 0; i < compenents.Length; i++)
        {
            compenents[i].HandleAbleToUse();
        }
    }

    public override void MyCastleTurnController_OnTurnMine(bool isTurnMine)
    {

        if (isTurnMine)
        {
            if (firstRound)
            {
                firstRound = false;
                return;
            }
            StartCoroutine(TurnCardsBack());
        }
        else
        {
        }
    }

    public IEnumerator TurnCardsBack()
    {
        var components = GetComponentsInChildren<GameCard3DUI>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].animator.SetTrigger("TurnBack");
            yield return new WaitForSeconds(0.2f);
        }
    }
}
