using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        int sandToSteal = enemy.sandResource >= 5 ? 5 : enemy.sandResource;
        int waterToSteal = enemy.waterResource >= 5 ? 5 : enemy.waterResource;
        int magicToSteal = enemy.magicResource >= 5 ? 5 : enemy.magicResource;

        executor.sandResource += sandToSteal;
        executor.waterResource += waterToSteal;
        executor.magicResource += magicToSteal;

        DecreaseResource(enemy, ResourceType.Sand, sandToSteal);
        DecreaseResource(enemy, ResourceType.Water, waterToSteal);
        DecreaseResource(enemy, ResourceType.Magic, magicToSteal);
    }
}
