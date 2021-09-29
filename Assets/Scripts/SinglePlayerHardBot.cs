using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SinglePlayerHardBot : BaseSinglePlayerBot
{
    public override BotResponse HandleTurn(CastleStats castleStat, int[] cardDeck)
    {
        List<GameCard> cards = new List<GameCard>();
        for (int i = 0; i < cardDeck.Length; i++)
        {
            cards.Add(CardManager.instance.GetCard(cardDeck[i]));
        }
        var cardArray = cards.OrderBy(x => x.botPriority).Reverse().ToArray();


        ResourceType firstType = ResourceType.Null;
        for (int i = 0; i < cardArray.Length; i++)
        {
            switch (cardArray[i].resourceType)
            {
                case ResourceType.Sand:
                    if (castleStat.sandResource >= cardArray[i].resourceCost && firstType != cardArray[i].resourceType)
                    {
                        return new BotResponse(cardArray[i], false);
                    }
                        break;
                case ResourceType.Water:
                    if (castleStat.waterResource >= cardArray[i].resourceCost && firstType != cardArray[i].resourceType)
                    {
                        return new BotResponse(cardArray[i], false);
                    }
                        break;
                case ResourceType.Magic:
                    if (castleStat.magicResource >= cardArray[i].resourceCost && firstType != cardArray[i].resourceType)
                    {
                        return new BotResponse(cardArray[i], false);
                    }
                        break;
                default:
                    break;
            }

            if(i == 0)
            {
                firstType = cardArray[i].resourceType;
            }
        }

        return new BotResponse(cardArray[cardArray.Length - 1], true);
    }
}
