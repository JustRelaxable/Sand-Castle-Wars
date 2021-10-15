using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Photon.Pun;

public class CastleTurnController : MonoBehaviour
{
    public bool myTurn = false;
    private TurnIndicatorUI turnIndicatorUI;
    public MeshRenderer sandMeshRenderer;
    public event Action<bool> OnTurnMine;
    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        turnIndicatorUI = FindObjectOfType<TurnIndicatorUI>();
    }

    [PunRPC]
    public void NextTurnRpc()
    {
        if (photonView.IsMine)
        {
            myTurn = true;
            OnTurnMine?.Invoke(true);
            turnIndicatorUI.SetIndicatorText(myTurn);
            HandleGlowing(myTurn);
        }
        else
        {
            var myCastle = FindObjectsOfType<CastleTurnController>().Single(x => x.photonView.IsMine);
            myCastle.myTurn = false;
            OnTurnMine?.Invoke(false);
            turnIndicatorUI.SetIndicatorText(myCastle.myTurn);
            //GameCardUI.RemoveSelectedCard();
            HandleGlowing(myCastle.myTurn);
        }
        FindObjectOfType<GameCardHolderUI>().HandleCardsAbleToUse();
    }

    private void HandleGlowing(bool myTurn)
    {
        //SetGlowing(1);
        //CastleTurnController otherCastle;
        //if(myTurn)
        //    otherCastle = FindObjectsOfType<CastleTurnController>().Single(x => !x.hasAuthority);
        //else
        //    otherCastle = FindObjectsOfType<CastleTurnController>().Single(x => x.hasAuthority);
        //otherCastle.SetGlowing(0);
    }

    public void SetGlowing(float value)
    {
        sandMeshRenderer.material.SetFloat("_ShouldGlow", value);
    }

    public void InvokeOnTurnMine()
    {
        OnTurnMine?.Invoke(true);
    }
}
