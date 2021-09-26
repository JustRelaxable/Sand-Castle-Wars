using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameCardUI : MonoBehaviour
{
    public static GameCardUI selectedCard;
    public Text resourceAmount;
    public Text cardName;
    public int cardID;
    public Button overButton;

    public Color normalColor;
    public Color darkenColor;
    bool ableToUseTheCard = false;
    public bool isLastCard = false;

    public Image overImage;
    public Image backgroundImage;
    private GameCard gameCard;
    private static GameCardUI settingsOpenedCard;
    public GameObject cardSettings;
    public Text cardDescription;

    public Sprite sandSprite;
    public Sprite waterSprite;
    public Sprite magicSprite;

    public void PrepareCard(GameCard gameCard,bool isLastCard)
    {
        cardID = CardManager.instance.GetIndex(gameCard);
        resourceAmount.text = gameCard.resourceCost.ToString();
        cardName.text = gameCard.cardName;
        this.gameCard = gameCard;
        this.isLastCard = isLastCard;
        cardDescription.text = gameCard.cardDescription;

        switch (gameCard.resourceType)
        {
            case ResourceType.Sand:
                backgroundImage.sprite = sandSprite;
                break;
            case ResourceType.Water:
                backgroundImage.sprite = waterSprite;
                break;
            case ResourceType.Magic:
                backgroundImage.sprite = magicSprite;
                break;
            default:
                break;
        }
        HandleAbleToUse();
    }

    public void HandleAbleToUse()
    {
        if (isLastCard)
            return;

        switch (gameCard.resourceType)
        {
            case ResourceType.Sand:
                if (gameCard.resourceCost > PlayerCastle.PlayerCastleStats.sandResource)
                {
                    overImage.color = darkenColor;
                    ableToUseTheCard = false;
                }

                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                }
                    
                break;
            case ResourceType.Water:
                if (gameCard.resourceCost > PlayerCastle.PlayerCastleStats.waterResource)
                {
                    overImage.color = darkenColor;
                    ableToUseTheCard = false;
                }

                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                }
                break;
            case ResourceType.Magic:
                if (gameCard.resourceCost > PlayerCastle.PlayerCastleStats.magicResource)
                {
                    overImage.color = darkenColor;
                    ableToUseTheCard = false;
                }
                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                }
                break;
            default:
                break;
        }
    }

    public void UseTheCard()
    {
        var myCastle = FindObjectsOfType<PlayerCards>().Single(x => x.hasAuthority);
        if (myCastle.GetComponent<CastleTurnController>().myTurn && ableToUseTheCard)
        {
            myCastle.CmdUseCard(cardID);
            selectedCard = this;
        }   
    }
    public void DiscardTheCard()
    {
        var myCastle = FindObjectsOfType<PlayerCards>().Single(x => x.hasAuthority);
        if (myCastle.GetComponent<CastleTurnController>().myTurn)
        {
            myCastle.CmdDiscardCard(cardID);
            selectedCard = this;
        }
    }

    public static void RemoveSelectedCard()
    {
        if(selectedCard != null)
            Destroy(selectedCard.gameObject);
    }

    public void OpenCardSettings()
    {
        if (cardSettings.activeInHierarchy)
        {
            CloseCardSettings();
            return;
        }
            
        cardSettings.SetActive(true);

        if (settingsOpenedCard != this)
            if(settingsOpenedCard != null)
                settingsOpenedCard.CloseCardSettings();

        settingsOpenedCard = this;
    }

    public void CloseCardSettings()
    {
        cardSettings.SetActive(false);
    }

    public static void CloseSettingsOpenedLastSelectedCard()
    {
        settingsOpenedCard?.CloseCardSettings();
    }

    public void DestroyTheCardForBonusCard()
    {
        List<GameCardUI> playerUsableCards = FindObjectsOfType<GameCardUI>().Where(x => !x.isLastCard).ToList();
        FindObjectOfType<BonusCardUI>().OnDestroyForBonusCardClicked();

        for (int i = 0; i < playerUsableCards.Count(); i++)
        {
            playerUsableCards[i].overButton.onClick.RemoveAllListeners();
            playerUsableCards[i].overButton.onClick.AddListener(OpenCardSettings);
        }

        Destroy(this.gameObject);
    }
}
