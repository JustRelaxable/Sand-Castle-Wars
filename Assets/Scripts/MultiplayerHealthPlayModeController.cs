using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerHealthPlayModeController : MonoBehaviour
{
    [SerializeField]
    Button multiplayerButton;

    private void OnEnable()
    {
        var multiplayerHealth = PlayerPrefs.GetInt("MultiplayerHealth", 5);
        if (multiplayerHealth <= 0)
            multiplayerButton.interactable = false;
    }

    public void OnMultiplayerEnter()
    {
        var multiplayerHealth = PlayerPrefs.GetInt("MultiplayerHealth", 5);
        PlayerPrefs.SetInt("MultiplayerHealth", multiplayerHealth - 1);

    }
}
