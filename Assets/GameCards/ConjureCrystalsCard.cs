using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureCrystalsCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.magicResource += 8;
        ChangeResource(executor, ResourceType.Magic, 8);
    }
}
