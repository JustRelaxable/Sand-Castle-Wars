using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSettingsUI : MonoBehaviour
{
    public Button discardButton;

    public void DiscardButtonSetInteractable(bool interactable)
    {
        discardButton.interactable = interactable;
    }
}
