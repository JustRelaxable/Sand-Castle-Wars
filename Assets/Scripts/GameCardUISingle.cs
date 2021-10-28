﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardUISingle : GameCard3DUI
{
    public override void OpenCardSettings()
    {
        overButton.enabled = false;
        FindObjectOfType<SnglePlayerGameManager>().ShowOffCardOpen(this.gameObject,overButton);
        animator.SetTrigger("OpenCardOptions");
        /*
        if (cardSettings.activeInHierarchy)
        {
            CloseCardSettings();
            return;
        }

        cardSettings.SetActive(true);

        if (settingsOpenedCard != this)
            if (settingsOpenedCard != null)
                settingsOpenedCard.CloseCardSettings();

        settingsOpenedCard = this;
        */
    }

    public override void UseTheCard()
    {
        selectedCard = this;
        //FindObjectOfType<SnglePlayerGameManager>().SetLastPlayedCardParentTransform(transform.parent);
        FindObjectOfType<SnglePlayerGameManager>().PlayCard(cardID,false);
    }

    public override void DiscardTheCard()
    {
        selectedCard = this;
        //FindObjectOfType<SnglePlayerGameManager>().SetLastPlayedCardParentTransform(transform.parent);
        FindObjectOfType<SnglePlayerGameManager>().PlayCard(cardID, true);
    }
}
