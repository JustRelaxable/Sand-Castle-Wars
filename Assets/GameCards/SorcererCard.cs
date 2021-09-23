﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererCard : GameCard
{
    public override void UseTheCard(CastleStats executor, CastleStats enemy)
    {
        base.UseTheCard(executor, enemy);
        executor.magicProduce += 1;
    }
}
