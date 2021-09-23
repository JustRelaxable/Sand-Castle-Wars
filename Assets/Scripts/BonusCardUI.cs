using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BonusCardUI : MonoBehaviour
{
    public GameObject cardHolder;
    private bool turnMine = false;
    [SerializeField]private ClientGameManager clientGameManager;
    [SerializeField]private BonusCardRewardedAds bonusCardRewardedAds;
    private Button bonusCardButton;
    public Text debugText;

    private void Awake()
    {
        
        bonusCardButton = GetComponent<Button>();
        

        //clientGameManager = Resources.FindObjectsOfTypeAll<ClientGameManager>()[0];
        
        clientGameManager.OnGameStarted += ClientGameManager_OnGameStarted;

        //bonusCardRewardedAds = Resources.FindObjectsOfTypeAll<BonusCardRewardedAds>()[0];
       

        SetButtonInteractable();
    }
    private void Update()
    {
        
    }
    private void ClientGameManager_OnGameStarted()
    {
        var turnControllers = FindObjectsOfType<CastleTurnController>();
        foreach (var turnController in turnControllers)
        {
            turnController.OnTurnMine += TurnController_OnTurnMine;
        }
    }

    private void TurnController_OnTurnMine(bool turnMine)
    {
        for (int i = 0; i < cardHolder.transform.childCount; i++)
        {
            Destroy(cardHolder.transform.GetChild(i).gameObject);
        }
        this.turnMine = turnMine;
        SetButtonInteractable();
    }

    private void SetButtonInteractable()
    {
        if(turnMine && bonusCardRewardedAds.adReady)
        {
            bonusCardButton.interactable = true;
        }
        else
        {
            bonusCardButton.interactable = false;
        }
    }

    public void OnBonusCardButtonClick()
    {
        turnMine = false;
        bonusCardRewardedAds.ShowBonusCardRewardedAd();
        var castle = FindObjectsOfType<PlayerCastle>().Single(x => x.hasAuthority);
        var netID = castle.netId;
        castle.CmdRequestBonusCard(netID);
        SetButtonInteractable();
    }


}
