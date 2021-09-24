using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCardAnimation : CardAnimation
{
    private CameraShake cameraShaker;


    public override void StartAction()
    {
        base.StartAction();
        cameraShaker = FindObjectOfType<CameraShake>();
        //StartCoroutine(cameraShaker.ShakeForSeconds(5f));
    }

    public void StartShake()
    {
        cameraShaker.StartShake();
    }

    public void StopShake()
    {
        cameraShaker.StopShake();
    }
}
