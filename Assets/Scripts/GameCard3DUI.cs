using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCard3DUI : GameCardUI
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void CloseCardBack()
    {
    }

    public override void OpenCardBack()
    {
    }

    public void OpenCardOptions()
    {

    }

    public void CloseCardOptions()
    {

    }
}
