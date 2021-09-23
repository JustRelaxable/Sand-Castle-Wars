using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaboteurCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseResource(enemy, ResourceType.Sand, 4);
        DecreaseResource(enemy, ResourceType.Water, 4);
        DecreaseResource(enemy, ResourceType.Magic, 4);
    }
}
