﻿using System.Collections;
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

    private void ActivateUI()
    {
        castleStatsUI.SetActive(true);
        turnIndicatorUI.SetActive(true);
        cameraCanvas.SetActive(true);
    }
}
