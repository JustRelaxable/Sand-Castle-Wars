using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    [SerializeField]
    SnglePlayerGameManager singlePlayerGameManager;
    [SerializeField]
    ClientGameManager clientGameManager;
    public void OnClicked()
    {
        switch (GameCardBase.GameMode)
        {
            case GameMode.Singleplayer:
                singlePlayerGameManager.ShowOffCardClose();
                break;
            case GameMode.Multiplayer:
                clientGameManager.ShowOffCardClose();
                break;
            default:
                break;
        }
    }
}
