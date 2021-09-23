using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        BasicAttack(enemy, 25);
    }
}
