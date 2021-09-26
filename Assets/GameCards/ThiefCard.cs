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

        ChangeResource(executor, ResourceType.Sand, sandToSteal);
        ChangeResource(executor, ResourceType.Water, waterToSteal);
        ChangeResource(executor, ResourceType.Magic, magicToSteal);

        ChangeResource(enemy, ResourceType.Sand, -sandToSteal);
        ChangeResource(enemy, ResourceType.Water, -waterToSteal);
        ChangeResource(enemy, ResourceType.Magic, -magicToSteal);
    }
}
