using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureWeaponsCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.waterResource += 8;
        ChangeResource(executor, ResourceType.Water, 8);
    }
}
