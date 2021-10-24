using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainmenuUIController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject[] mainMenuUIs;

    [SerializeField]
    GameObject homeButton;

    [SerializeField]
    GameObject title;

    public void OpenMainMenuUI()
    {
        foreach (var item in mainMenuUIs)
        {
            item.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        homeButton.SetActive(false);
        title.SetActive(false);
    }
}
