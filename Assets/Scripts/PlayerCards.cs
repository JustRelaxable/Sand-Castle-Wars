using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;

public class PlayerCards : MonoBehaviour
{
    public int[] cardDeck = new int[8];
    private GameCardHolderUI gameCardHolderUI;
    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        for (int i = 0; i < cardDeck.Length; i++)
        {
            cardDeck[i] = -1;
        }
        gameCardHolderUI = FindObjectOfType<GameCardHolderUI>();
    }
    public void CmdUseCard(int cardID)
    {
        //GameManager.instance.UseCard(netId,cardID);
    }
    public void CmdDiscardCard(int cardID)
    {
        //GameManager.instance.DiscardCard(netId,cardID);
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
    public void RpcTakeCard(int cardID)
    {
        //if (!hasAuthority)
        //    return;
        ////gameCardHolderUI.InstantiateCard(cardID);
        //var gameCard = gameCardHolderUI.InstantiateCardAndReturn(cardID);
        ////gameCard.GetComponent<GameCardUI>().OpenCardBack();
        //FindObjectOfType<ClientGameManager>().TakeCardFromDeck(gameCard);
    }

    public void RpcTakeBonusCard(int cardID)
    {
        //if (!hasAuthority)
        //    return;
        //(gameCardHolderUI).InstantiateBonusCard(cardID);
    }
}
