using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.waterProduce += 1;
        ChangeProduce(executor, ResourceType.Water, 1);
    }
}
