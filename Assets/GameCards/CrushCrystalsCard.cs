using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushCrystalsCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        DecreaseResource(enemy, ResourceType.Magic, 8);
    }
}
