using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushWeaponsCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        enemy.waterResource -= 8;
        if (enemy.waterResource < 0)
            enemy.waterResource = 0;
    }
}
