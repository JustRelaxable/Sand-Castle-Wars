using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public UnityAction<string, int> testUnityAction;

    private void Awake()
    {
        testUnityAction += Za;
    }

    public void Za(string x,int y)
    {

    }
}
