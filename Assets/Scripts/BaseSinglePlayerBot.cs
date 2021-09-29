using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSinglePlayerBot
{
    public List<int> deck = new List<int>();
    public abstract BotResponse HandleTurn(CastleStats castleStat, int[] cardDeck);
}
