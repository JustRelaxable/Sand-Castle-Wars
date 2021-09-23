using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseResource(enemy, ResourceType.Sand, 1);
        DecreaseResource(enemy, ResourceType.Water, 1);
        DecreaseResource(enemy, ResourceType.Magic, 1);

        DecreaseProduce(enemy, ResourceType.Sand, 1);
        DecreaseProduce(enemy, ResourceType.Water, 1);
        DecreaseProduce(enemy, ResourceType.Magic, 1);

        DecreaseHeightOfBuilding(enemy, BuildingType.Castle, 1);
        DecreaseHeightOfBuilding(enemy, BuildingType.Wall, 1);

        executor.sandResource += 1;
        executor.sandProduce += 1;

        executor.waterResource += 1;
        executor.waterProduce += 1;

        executor.magicResource += 1;
        executor.magicProduce += 1;

        executor.wallHeight += 1;
        executor.castleHeight += 1;
    }
}
