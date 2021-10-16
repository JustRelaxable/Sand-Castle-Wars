using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PlayWaiter : MonoBehaviour
{
    [SerializeField]
    private ClientGameManager clientGameManager;

    public bool canPlay = false;

    private void Awake()
    {
        clientGameManager.OnGameStarted += ClientGameManager_OnGameStarted;
    }

    private void ClientGameManager_OnGameStarted()
    {
        var myCastleTurnController = FindObjectsOfType<CastleTurnController>().Single(x => x.GetComponent<PhotonView>().IsMine);
        myCastleTurnController.OnTurnMine += MyCastleTurnController_OnTurnMine;
    }

    private void MyCastleTurnController_OnTurnMine(bool obj)
    {
        if (obj)
        {
            StartCoroutine(WaitForPlay(3.5f));
        }
    }

    public IEnumerator WaitForPlay(float waitTime)
    {
        canPlay = false;
        yield return new WaitForSeconds(waitTime);
        canPlay = true;
    }
}
