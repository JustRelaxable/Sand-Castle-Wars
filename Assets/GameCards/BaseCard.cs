﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        //executor.castleHeight += 2;
        ChangeHeightOfBuilding(executor, BuildingType.Castle, 2);
    }
}
