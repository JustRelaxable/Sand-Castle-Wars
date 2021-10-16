using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    void Update()
    {
        float t = -Time.time % 360;
        RenderSettings.skybox.SetFloat("_Rotation", t);
    }
}
