using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotResponse
{
    public GameCard gameCard;
    public bool isDiscarded;
    public BotResponse(GameCard gameCard,bool isDiscarded)
    {
        this.gameCard = gameCard;
        this.isDiscarded = isDiscarded;
    }
}
