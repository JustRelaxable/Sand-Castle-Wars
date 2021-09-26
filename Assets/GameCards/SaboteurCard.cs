using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaboteurCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        ChangeResource(enemy, ResourceType.Sand, -4);
        ChangeResource(enemy, ResourceType.Water, -4);
        ChangeResource(enemy, ResourceType.Magic, -4);
    }
}
