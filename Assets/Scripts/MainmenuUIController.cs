using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainmenuUIController : MonoBehaviour
{
    [SerializeField]
    GameObject[] mainMenuUIs;

    public void OpenMainMenuUI()
    {
        foreach (var item in mainMenuUIs)
        {
            item.SetActive(true);
        }
    }
}
