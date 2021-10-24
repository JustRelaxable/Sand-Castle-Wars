using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GameCardUI : MonoBehaviour
{
    public static GameCardUI selectedCard;
    public TextMeshProUGUI resourceAmount;
    public TextMeshProUGUI cardName;
    public LocalizeStringEvent cardNameString;
    public TextMeshProUGUI cardDescription;
    public LocalizeStringEvent cardDescriptionString;
    public int cardID;
    public Button overButton;

    public Color normalColor;
    public Color darkenColor;
    bool ableToUseTheCard = false;
    public bool isLastCard = false;

    public Image overImage;
    public Image backgroundImage;
    public Image cardIcon;
    private GameCard gameCard;
    protected static GameCardUI settingsOpenedCard;
    public GameObject cardSettings;


    public Sprite sandSprite;
    public Sprite waterSprite;
    public Sprite magicSprite;

    public GameObject cardBack;
    public GameObject discarded;

    public Button useButton;
    public GameObject numberTextPrefab;
    public float cardWidth;
    public float cardHeight;

    private string translations = "Translations";

    public void PrepareCard(GameCard gameCard,bool isLastCard)
    {
        cardNameString.StringReference.TableReference = translations;
        cardDescriptionString.StringReference.TableReference = translations;

        cardID = CardManager.instance.GetIndex(gameCard);
        resourceAmount.text = gameCard.resourceCost.ToString();


        var localizedCardName = GetLocalizedName(gameCard.cardName);
        cardNameString.StringReference.TableEntryReference = localizedCardName;
        //cardName.text = gameCard.cardName;

        this.gameCard = gameCard;
        this.isLastCard = isLastCard;
        //cardDescription.text = gameCard.cardDescription;
        localizedCardName = GetLocalizedDescription(gameCard.cardName);
        cardDescriptionString.StringReference.TableEntryReference = localizedCardName;
        if(gameCard.cardIcon != null)
            cardIcon.sprite = gameCard.cardIcon;

        for (int i = 0; i < gameCard.cardNumberTexts.Length; i++)
        {
            var go = Instantiate(numberTextPrefab, backgroundImage.transform);
            var x = Mathf.Lerp(-cardWidth, cardWidth, gameCard.cardNumberTexts[i].textX);
            var y = Mathf.Lerp(-cardHeight, cardHeight, gameCard.cardNumberTexts[i].textY);
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(x,y,0);
            var tmpro = go.GetComponent<TextMeshProUGUI>();
            tmpro.text = gameCard.cardNumberTexts[i].cardText;
            tmpro.color = gameCard.cardNumberTexts[i].textColor;
        }

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
                    useButton.interactable = false;
                }

                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                    useButton.interactable = true;
                }
                    
                break;
            case ResourceType.Water:
                if (gameCard.resourceCost > PlayerCastle.PlayerCastleStats.waterResource)
                {
                    overImage.color = darkenColor;
                    ableToUseTheCard = false;
                    useButton.interactable = false;
                }

                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                    useButton.interactable = true;
                }
                break;
            case ResourceType.Magic:
                if (gameCard.resourceCost > PlayerCastle.PlayerCastleStats.magicResource)
                {
                    overImage.color = darkenColor;
                    ableToUseTheCard = false;
                    useButton.interactable = false;
                }
                else
                {
                    overImage.color = normalColor;
                    ableToUseTheCard = true;
                    useButton.interactable = true;
                }
                break;
            default:
                break;
        }
    }

    public virtual void UseTheCard()
    {
        var myCastlePlayerCard = FindObjectsOfType<PlayerCards>().Single(x => x.photonView.IsMine);
        if (myCastlePlayerCard.GetComponent<CastleTurnController>().myTurn && ableToUseTheCard)
        {
            //myCastle.UseCardRpc(cardID);
            selectedCard = this;
            //FindObjectOfType<ClientGameManager>().SetLastPlayedCardParentTransform(transform.parent);
            myCastlePlayerCard.photonView.RPC("UseCardRpc", RpcTarget.MasterClient, myCastlePlayerCard.photonView.ViewID, cardID);

            
        }
    }
    public virtual void DiscardTheCard()
    {
        var myCastlePlayerCards = FindObjectsOfType<PlayerCards>().Single(x => x.photonView.IsMine);
        if (myCastlePlayerCards.GetComponent<CastleTurnController>().myTurn)
        {
            selectedCard = this;
            //FindObjectOfType<ClientGameManager>().SetLastPlayedCardParentTransform(transform.parent);
            //myCastle.DiscardCardRpc(cardID);
        }
        myCastlePlayerCards.photonView.RPC("DiscardCardRpc", RpcTarget.MasterClient, myCastlePlayerCards.photonView.ViewID, cardID);
    }

    public static void RemoveSelectedCard()
    {
        if(selectedCard != null)
            Destroy(selectedCard.gameObject);
    }

    public virtual void OpenCardSettings()
    {
        bool adsWatchedOnBothPlayers = FindObjectOfType<RoundBasedAds>().adsWatchedOnBothPlayers;
        if (!FindObjectOfType<PlayWaiter>().canPlay)
        {
            return;
        }
        if (!adsWatchedOnBothPlayers)
        {
            return;
        }

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

    public virtual void OpenCardBack()
    {
        cardBack.SetActive(true);
    }

    public virtual void CloseCardBack()
    {
        cardBack.SetActive(false);
    }

    public void OpenDiscarded()
    {
        discarded.SetActive(true);
    }

    public void DeactivateButton()
    {
        overButton.enabled = false;
    }

    public string GetLocalizedName(string cardName)
    {
        var upper = cardName.ToUpper();
        var fixedCardName = upper.Replace(' ', '_');
        fixedCardName = fixedCardName.Replace('İ', 'I');
        return fixedCardName + "_CARD_NAME";
    }
    public string GetLocalizedDescription(string cardName)
    {
        var upper = cardName.ToUpper();
        var fixedCardName = upper.Replace(' ', '_');
        fixedCardName = fixedCardName.Replace('İ', 'I');
        return fixedCardName + "_CARD_DESCRIPTION";
    }
}
