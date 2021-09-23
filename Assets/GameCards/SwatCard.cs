using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseHeightOfBuilding(enemy, BuildingType.Wall, 10);
    }
}
