using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCards : MonoBehaviourPun
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
    [PunRPC]
    public void UseCardRpc(int viewID,int cardID)
    {     
        GameManager.instance.UseCard(viewID, cardID);
    }
    [PunRPC]
    public void DiscardCardRpc(int viewID,int cardID)
    {
        GameManager.instance.DiscardCard(viewID,cardID);
    }

    private void Update()
    {
        //if (!hasAuthority)
        //    return;
    }

    [PunRPC]
    public void SetCardDeckRpc(int[] cardIDs)
    {
        if (!photonView.IsMine)
            return;
        cardDeck = cardIDs;
        foreach (var cardID in cardDeck)
        {
            gameCardHolderUI.InstantiateCard(cardID);
        }
    }
    [PunRPC]
    public void TakeCardRpc(int cardID)
    {
        if (!photonView.IsMine)
            return;
        //gameCardHolderUI.InstantiateCard(cardID);
        var gameCard = gameCardHolderUI.InstantiateCardAndReturn(cardID,false);
        //gameCard.GetComponent<GameCardUI>().OpenCardBack();
        FindObjectOfType<ClientGameManager>().TakeCardFromDeck(gameCard);
    }

    public void RpcTakeBonusCard(int cardID)
    {
        //if (!hasAuthority)
        //    return;
        //(gameCardHolderUI).InstantiateBonusCard(cardID);
    }
}
