using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushWeaponsCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        ChangeResource(enemy, ResourceType.Water, -8);
    }
}
