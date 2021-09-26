using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WainCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.castleHeight += 8;
        ChangeHeightOfBuilding(executor, BuildingType.Castle, 8);
        ChangeHeightOfBuilding(enemy, BuildingType.Castle, -4);
    }
}
