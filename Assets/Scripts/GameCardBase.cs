using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardBase : MonoBehaviour
{
    private static GameCardVariation cardVariation;
    public static GameMode GameMode { get => GetGameMode();}

    static GameCardBase()
    {
        cardVariation = new GameCardMulti();
    }

    private static GameMode GetGameMode()
    {
        if(cardVariation is GameCardSingle)
        {
            return GameMode.Singleplayer;
        }
        else
        {
            return GameMode.Multiplayer;
        }
    }

    public static void ChangeGameCardVariation(GameCardVariation gcv)
    {
        cardVariation = gcv;
    }

    public void UseTheCard(CastleStats executor, CastleStats enemy,GameCard gameCard)
    {
        cardVariation.UseTheCard(executor, enemy,gameCard);
    }

    public void HandleCardCost(CastleStats executor,GameCard gameCard)
    {
        cardVariation.HandleCardCost(executor,gameCard);
    }

    public void BasicAttack(CastleStats enemy, int attackTime)
    {
        cardVariation.BasicAttack(enemy, attackTime);
    }

    public void ChangeResource(CastleStats target, ResourceType type, int amount)
    {
        cardVariation.ChangeResource(target, type, amount);
    }

    public void ChangeProduce(CastleStats target, ResourceType type, int amount)
    {
        cardVariation.ChangeProduce(target, type, amount);
    }

    public void ChangeHeightOfBuilding(CastleStats target, BuildingType type, int amount)
    {
        cardVariation.ChangeHeightOfBuilding(target, type, amount);
    }
}

public enum GameMode
{
    Singleplayer,Multiplayer
}