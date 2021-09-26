using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.castleHeight += 20;
        ChangeHeightOfBuilding(executor, BuildingType.Castle, 20);
    }
}
