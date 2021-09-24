using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushBricksCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseResource(enemy, ResourceType.Sand, 8);
    }
}
