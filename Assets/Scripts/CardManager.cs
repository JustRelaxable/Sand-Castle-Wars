using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public GameCard[] cards;

    private void Awake()
    {
        instance = this;

        cards = transform.GetComponentsInChildren<GameCard>();
    }

    public GameCard GetCard(int index)
    {
        return cards[index];
    }

    public int GetIndex(GameCard card)
    {
        return Array.FindIndex<GameCard>(cards, x => x == card);
    }
}
