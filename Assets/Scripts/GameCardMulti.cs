using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameCardMulti : GameCardVariation
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy,GameCard gameCard)
    {
        HandleCardCost(executor,gameCard);
    }

    public override void HandleCardCost(CastleStats executor,GameCard gameCard)
    {
        switch (gameCard.resourceType)
        {
            case ResourceType.Sand:
                executor.sandResource -= gameCard.resourceCost;
                //executor.SetResourceRpc(((byte)gameCard.resourceType), executor.sandResource);
                executor.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)gameCard.resourceType, executor.sandResource);
                break;
            case ResourceType.Water:
                executor.waterResource -= gameCard.resourceCost;
                //executor.SetResourceRpc(((byte)gameCard.resourceType), executor.waterResource);
                executor.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)gameCard.resourceType, executor.waterResource);
                break;
            case ResourceType.Magic:
                executor.magicResource -= gameCard.resourceCost;
                //executor.SetResourceRpc(((byte)gameCard.resourceType), executor.magicResource);
                executor.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)gameCard.resourceType, executor.magicResource);
                break;
            default:
                break;
        }
    }

    public override void BasicAttack(CastleStats enemy, int attackTime)
    {
        //enemy.BasicAttackRpc(attackTime);
        enemy.photonView.RPC("BasicAttackRpc", RpcTarget.All, attackTime);
    }

    public override void ChangeResource(CastleStats target, ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Sand:
                target.sandResource += amount;
                if (target.sandResource < 0)
                    target.sandResource = 0;
                //target.SetResourceRpc(((byte)type), target.sandResource);
                target.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)type, target.sandResource);
                break;
            case ResourceType.Water:
                target.waterResource += amount;
                if (target.waterResource < 0)
                    target.waterResource = 0;
                //target.SetResourceRpc(((byte)type), target.waterResource);
                target.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)type, target.waterResource);
                break;
            case ResourceType.Magic:
                target.magicResource += amount;
                if (target.magicResource < 0)
                    target.magicResource = 0;
                //target.SetResourceRpc(((byte)type), target.magicResource);
                target.photonView.RPC("SetResourceRpc", RpcTarget.All, (byte)type, target.magicResource);
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
                //target.SetProduceRpc(((byte)type), target.sandProduce);
                target.photonView.RPC("SetProduceRpc", RpcTarget.All, (byte)type, target.sandProduce);
                break;
            case ResourceType.Water:
                target.waterProduce += amount;
                if (target.waterProduce < 1)
                    target.waterProduce = 1;
                //target.SetProduceRpc(((byte)type), target.waterProduce);
                target.photonView.RPC("SetProduceRpc", RpcTarget.All, (byte)type, target.waterProduce);
                break;
            case ResourceType.Magic:
                target.magicProduce += amount;
                if (target.magicProduce < 1)
                    target.magicProduce = 1;
                //target.SetProduceRpc(((byte)type), target.magicProduce);
                target.photonView.RPC("SetProduceRpc", RpcTarget.All, (byte)type, target.magicProduce);
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
                //target.SetHeightOfBuildingRpc(((byte)type), target.wallHeight);
                target.photonView.RPC("SetHeightOfBuildingRpc", RpcTarget.All, (byte)type, target.wallHeight);
                break;
            case BuildingType.Castle:
                target.castleHeight += amount;
                if (target.castleHeight < 0)
                    target.castleHeight = 0;
                //target.SetHeightOfBuildingRpc(((byte)type), target.castleHeight);
                target.photonView.RPC("SetHeightOfBuildingRpc", RpcTarget.All, (byte)type, target.castleHeight);
                break;
            default:
                break;
        }
    }
}
