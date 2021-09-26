using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureBricksCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.sandResource += 8;
        ChangeResource(executor, ResourceType.Sand, 8);
    }
}
