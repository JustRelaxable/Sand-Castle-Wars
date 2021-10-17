using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using Photon.Pun;

public class CameraAnimatorManager : MonoBehaviour
{
    Animator animator;

    public GameObject castleStatsUI;
    public GameObject turnIndicatorUI;
    public GameObject cameraCanvas;
    public GameObject mainMenu;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeGameMode(int gameMode)
    {
        animator.SetInteger("GameMode", gameMode);
    }

    public void StartGameAnimation()
    {
        animator.SetTrigger("GameStarts");
    }

    public void MenuBehaviourStateExit()
    {
        HandleGameModeScene();
        animator.GetComponent<CameraShake>().enabled = true;
        animator.applyRootMotion = true;
    }

    private void HandleGameModeScene()
    {
        var gameMode = animator.GetInteger("GameMode");
        switch (gameMode)
        {
            case 0:
                ActivateUI();
                FindObjectOfType<SnglePlayerGameManager>().SetupScene();
                break;
            case 1:
                ActivateUI();
                PhotonNetwork.Instantiate("PlayerNetworkObject", Vector3.zero, Quaternion.identity);
                //NetworkManager.singleton.StartClient(mathcInfo);
                break;
            case 2:
                ActivateUI();
                PhotonNetwork.Instantiate("PlayerNetworkObject", Vector3.zero, Quaternion.identity);
                //NetworkServer.Listen(mathcInfo, 9000);
                //NetworkManager.singleton.StartHost(mathcInfo);
                break;
            default:
                break;
        }
    }

    public void HandleGameModeClose()
    {
        var gameMode = animator.GetInteger("GameMode");
        FindObjectOfType<RoundBasedAds>().ResetAdCount();
        switch (gameMode)
        {
            case 0:
                //might need to clean the round based ad count
                //FindObjectOfType<SnglePlayerGameManager>().adManager.SetActive(false);
                break;
            case 1:
                PhotonNetwork.LeaveRoom();
                break;
            case 2:
                PhotonNetwork.LeaveRoom();
                break;
            default:
                break;
        }

        //animator.SetInteger("GameMode", -1);
    }

    private void ActivateUI()
    {
        castleStatsUI.SetActive(true);
        turnIndicatorUI.SetActive(true);
        cameraCanvas.SetActive(true);
    }
    public void ClearGameScene()
    {
        castleStatsUI.SetActive(false);
        turnIndicatorUI.SetActive(false);
        CardHolderClear();
        DestroyCastles();
        cameraCanvas.SetActive(false);

        //HandleGameModeClose();
    }

    private void CardHolderClear()
    {
        var holders = FindObjectsOfType<GameCardHolderUI>();

        foreach (var holder in holders)
        {
            holder.ClearAllCards();
            holder.ResetFirstPlay();
        }
    }

    private void DestroyCastles()
    {
        var castles = FindObjectsOfType<PlayerCastle>();

        for (int i = 0; i < castles.Length; i++)
        {
            Destroy(castles[i].gameObject);
        }
    }

    public void ReturnToMainMenu()
    {
        GameCardBase.ChangeGameCardVariation(new GameCardMulti());
        mainMenu.SetActive(true);
    }

    private void OnDisconnectedFromServer(NetworkConnection info)
    {
        Debug.Log("xx");
    }
}
