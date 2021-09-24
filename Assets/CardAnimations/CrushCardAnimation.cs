using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushCardAnimation : CardAnimation
{
    public Color color;
    public ParticleSystem mainParticle;

    public override void StartAction()
    {
        base.StartAction();
        mainParticle.startColor = color;
    }
}
