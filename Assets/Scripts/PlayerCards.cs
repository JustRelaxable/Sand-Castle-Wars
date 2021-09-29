using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCards : NetworkBehaviour
{
    public int[] cardDeck = new int[8];
    private GameCardHolderUI gameCardHolderUI;
    private void Awake()
    {
        for (int i = 0; i < cardDeck.Length; i++)
        {
            cardDeck[i] = -1;
        }
        gameCardHolderUI = FindObjectOfType<GameCardHolderUI>();
    }

    [Command]
    public void CmdUseCard(int cardID)
    {
        GameManager.instance.UseCard(netId,cardID);
    }

    [Command]
    public void CmdDiscardCard(int cardID)
    {
        GameManager.instance.DiscardCard(netId,cardID);
    }

    private void Update()
    {
        if (!hasAuthority)
            return;
    }

    [ClientRpc]
    public void RpcSetCardDeck(int[] cardIDs)
    {
        if (!hasAuthority)
            return;
        cardDeck = cardIDs;
        foreach (var cardID in cardDeck)
        {
            gameCardHolderUI.InstantiateCard(cardID);
        }    
    }

    [ClientRpc]
    public void RpcTakeCard(int cardID)
    {
        if (!hasAuthority)
            return;
        //gameCardHolderUI.InstantiateCard(cardID);
        var gameCard = gameCardHolderUI.InstantiateCardAndReturn(cardID);
        //gameCard.GetComponent<GameCardUI>().OpenCardBack();
        FindObjectOfType<ClientGameManager>().TakeCardFromDeck(gameCard);
    }

    [ClientRpc]
    public void RpcTakeBonusCard(int cardID)
    {
        if (!hasAuthority)
            return;
        (gameCardHolderUI).InstantiateBonusCard(cardID);
    }
}
