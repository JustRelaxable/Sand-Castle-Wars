using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardSingle : GameCardVariation
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy, GameCard gameCard)
    {
        HandleCardCost(executor, gameCard);
    }

    public override void HandleCardCost(CastleStats executor, GameCard gameCard)
    {
        switch (gameCard.resourceType)
        {
            case ResourceType.Sand:
                executor.sandResource -= gameCard.resourceCost;
                executor.SetResource(((byte)gameCard.resourceType), executor.sandResource);
                break;
            case ResourceType.Water:
                executor.waterResource -= gameCard.resourceCost;
                executor.SetResource(((byte)gameCard.resourceType), executor.waterResource);
                break;
            case ResourceType.Magic:
                executor.magicResource -= gameCard.resourceCost;
                executor.SetResource(((byte)gameCard.resourceType), executor.magicResource);
                break;
            default:
                break;
        }
    }

    public override void ChangeResource(CastleStats target, ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Sand:
                target.sandResource += amount;
                if (target.sandResource < 0)
                    target.sandResource = 0;
                target.SetResource(((byte)type), target.sandResource);
                break;
            case ResourceType.Water:
                target.waterResource += amount;
                if (target.waterResource < 0)
                    target.waterResource = 0;
                target.SetResource(((byte)type), target.waterResource);
                break;
            case ResourceType.Magic:
                target.magicResource += amount;
                if (target.magicResource < 0)
                    target.magicResource = 0;
                target.SetResource(((byte)type), target.magicResource);
                break;
            default:
                break;
        }
    }

    public override void ChangeProduce(CastleStats target, ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Sand:
                target.sandProduce += amount;
                if (target.sandProduce < 1)
                    target.sandProduce = 1;
                target.SetProduce(((byte)type), target.sandProduce);
                break;
            case ResourceType.Water:
                target.waterProduce += amount;
                if (target.waterProduce < 1)
                    target.waterProduce = 1;
                target.SetProduce(((byte)type), target.waterProduce);
                break;
            case ResourceType.Magic:
                target.magicProduce += amount;
                if (target.magicProduce < 1)
                    target.magicProduce = 1;
                target.SetProduce(((byte)type), target.magicProduce);
                break;
            default:
                break;
        }
    }

    public override void ChangeHeightOfBuilding(CastleStats target, BuildingType type, int amount)
    {
        switch (type)
        {
            case BuildingType.Wall:
                target.wallHeight += amount;
                if (target.wallHeight < 0)
                    target.wallHeight = 0;
                target.SetHeightOfBuilding(((byte)type), target.wallHeight);
                break;
            case BuildingType.Castle:
                target.castleHeight += amount;
                if (target.castleHeight < 0)
                    target.castleHeight = 0;
                target.SetHeightOfBuilding(((byte)type), target.castleHeight);
                break;
            default:
                break;
        }
    }

    public override void BasicAttack(CastleStats enemy, int attackTime)
    {
        enemy.BasicAttack(attackTime);
    }
}
