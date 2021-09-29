
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardVariation
{
    public virtual void UseTheCard(CastleStats executor, CastleStats enemy,GameCard gameCard)
    {
        HandleCardCost(executor,gameCard);
    }

    public virtual void HandleCardCost(CastleStats executor,GameCard gameCard)
    {
    }

    public virtual void BasicAttack(CastleStats enemy, int attackTime)
    {
    }

    public virtual void ChangeResource(CastleStats target, ResourceType type, int amount)
    {
    }

    public virtual void ChangeProduce(CastleStats target, ResourceType type, int amount)
    {
    }

    public virtual void ChangeHeightOfBuilding(CastleStats target, BuildingType type, int amount)
    {
    }
}
