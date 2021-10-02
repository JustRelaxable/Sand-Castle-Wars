using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

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

    public void MenuBehaviourStateExit(MatchInfo mathcInfo = null)
    {
        HandleGameModeScene(mathcInfo);
        animator.GetComponent<CameraShake>().enabled = true;
        animator.applyRootMotion = true;
    }

    private void HandleGameModeScene(MatchInfo mathcInfo)
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
                NetworkManager.singleton.StartClient(mathcInfo);
                break;
            case 2:
                ActivateUI();
                NetworkServer.Listen(mathcInfo, 9000);
                NetworkManager.singleton.StartHost(mathcInfo);
                break;
            default:
                break;
        }
    }

    private void HandleGameModeClose()
    {
        var gameMode = animator.GetInteger("GameMode");
        switch (gameMode)
        {
            case 0:
                FindObjectOfType<SnglePlayerGameManager>().adManager.SetActive(false);
                break;
            case 1:
                NetworkManager.singleton.StopClient();
                break;
            case 2:
                NetworkManager.singleton.StopHost();
                break;
            default:
                break;
        }

        animator.SetInteger("GameMode", -1);
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

        HandleGameModeClose();
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
