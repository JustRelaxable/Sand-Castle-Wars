using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureCardAnimation : CardAnimation
{
    public Color color;
    public ParticleSystem mainParticle;
    public ParticleSystem sparkleParticle;

    public override void StartAction()
    {
        base.StartAction();
        mainParticle.startColor = color;
        sparkleParticle.startColor = color;
    }
}
