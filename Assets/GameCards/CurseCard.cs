using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        ChangeResource(enemy, ResourceType.Sand, -1);
        ChangeResource(enemy, ResourceType.Water, -1);
        ChangeResource(enemy, ResourceType.Magic, -1);

        ChangeProduce(enemy, ResourceType.Sand, -1);
        ChangeProduce(enemy, ResourceType.Water, -1);
        ChangeProduce(enemy, ResourceType.Magic, -1);

        ChangeHeightOfBuilding(enemy, BuildingType.Castle, -1);
        ChangeHeightOfBuilding(enemy, BuildingType.Wall, -1);

        //executor.sandResource += 1;
        //executor.sandProduce += 1;
        ChangeResource(executor, ResourceType.Sand, 1);
        ChangeProduce(executor, ResourceType.Sand, 1);

        //executor.waterResource += 1;
        //executor.waterProduce += 1;
        ChangeResource(executor, ResourceType.Water, 1);
        ChangeProduce(executor, ResourceType.Water, 1);

        //executor.magicResource += 1;
        //executor.magicProduce += 1;
        ChangeResource(executor, ResourceType.Magic, 1);
        ChangeProduce(executor, ResourceType.Magic, 1);

        //executor.wallHeight += 1;
        //executor.castleHeight += 1;
        ChangeHeightOfBuilding(executor, BuildingType.Wall, 1);
        ChangeHeightOfBuilding(executor, BuildingType.Castle, 1);
    }
}
