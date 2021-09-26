using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCard : MonoBehaviour,ICard
{
    public GameObject animationPrefab;
    public string cardName;
    public int resourceCost;
    public ResourceType resourceType;
    [TextArea]
    public string cardDescription;
    public virtual void UseTheCard(CastleStats executor,CastleStats enemy)
    {
        HandleCardCost(executor);
    }

    public void HandleCardCost(CastleStats executor)
    {
        switch (resourceType)
        {
            case ResourceType.Sand:
                executor.sandResource -= resourceCost;
                executor.RpcSetResource(((byte)resourceType), executor.sandResource);
                break;
            case ResourceType.Water:
                executor.waterResource -= resourceCost;
                executor.RpcSetResource(((byte)resourceType), executor.waterResource);
                break;
            case ResourceType.Magic:
                executor.magicResource -= resourceCost;
                executor.RpcSetResource(((byte)resourceType), executor.magicResource);
                break;
            default:
                break;
        }
    }

    public void BasicAttack(CastleStats enemy, int attackTime)
    {
        enemy.RpcBasicAttack(attackTime);
    }

    public void ChangeResource(CastleStats target,ResourceType type,int amount)
    { 
        switch (type)
        {
            case ResourceType.Sand:
                target.sandResource += amount;
                if (target.sandResource < 0)
                    target.sandResource = 0;
                target.RpcSetResource(((byte)type), target.sandResource);
                break;
            case ResourceType.Water:
                target.waterResource += amount;
                if (target.waterResource < 0)
                    target.waterResource = 0;
                target.RpcSetResource(((byte)type), target.waterResource);
                break;
            case ResourceType.Magic:
                target.magicResource += amount;
                if (target.magicResource < 0)
                    target.magicResource = 0;
                target.RpcSetResource(((byte)type), target.magicResource);
                break;
            default:
                break;
        }
    }

    public void ChangeProduce(CastleStats target, ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Sand:
                target.sandProduce += amount;
                if (target.sandProduce < 1)
                    target.sandProduce = 1;
                target.RpcSetProduce(((byte)type), target.sandProduce);
                break;
            case ResourceType.Water:
                target.waterProduce += amount;
                if (target.waterProduce < 1)
                    target.waterProduce = 1;
                target.RpcSetProduce(((byte)type), target.waterProduce);
                break;
            case ResourceType.Magic:
                target.magicProduce += amount;
                if (target.magicProduce < 1)
                    target.magicProduce = 1;
                target.RpcSetProduce(((byte)type), target.magicProduce);
                break;
            default:
                break;
        }
    }

    public void ChangeHeightOfBuilding(CastleStats target,BuildingType type,int amount)
    {
        switch (type)
        {
            case BuildingType.Wall:
                target.wallHeight += amount;
                if (target.wallHeight < 0)
                    target.wallHeight = 0;
                target.RpcSetHeightOfBuilding(((byte)type), target.wallHeight);
                break;
            case BuildingType.Castle:
                target.castleHeight += 0;
                if (target.castleHeight < 0)
                    target.castleHeight = 0;
                target.RpcSetHeightOfBuilding(((byte)type), target.castleHeight);
                break;
            default:
                break;
        }
    }
}

public enum ResourceType
{
    Sand, Water, Magic
}

public enum BuildingType
{
    Wall,Castle
}