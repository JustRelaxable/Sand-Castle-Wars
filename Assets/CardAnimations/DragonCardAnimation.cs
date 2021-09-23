using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCardAnimation : CardAnimation
{
    public override void StartAction()
    {
        base.StartAction();
        var cameraShaker = FindObjectOfType<CameraShake>();
        StartCoroutine(cameraShaker.ShakeForSeconds(5f));
    }
}
