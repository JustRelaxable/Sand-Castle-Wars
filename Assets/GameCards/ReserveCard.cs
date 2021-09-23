using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseHeightOfBuilding(executor, BuildingType.Wall, 4);
        executor.castleHeight += 8;
    }
}
