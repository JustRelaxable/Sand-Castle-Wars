using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushBricksCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        enemy.sandResource -= 8;
        if (enemy.sandResource < 0)
            enemy.sandResource = 0;
    }
}
