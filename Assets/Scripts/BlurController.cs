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
                singlePlayerGameManager.ShowOffCardClose(false);
                break;
            case GameMode.Multiplayer:
                clientGameManager.ShowOffCardClose(false);
                break;
            default:
                break;
        }
    }
}
