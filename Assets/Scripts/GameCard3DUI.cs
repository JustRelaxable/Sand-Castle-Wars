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
    public override void OpenCardSettings()
    {
        overButton.enabled = false;
        if (!FindObjectOfType<PlayWaiter>().canPlay)
        {
            return;
        }
        FindObjectOfType<ClientGameManager>().ShowOffCardOpen(this.gameObject,overButton);
        animator.SetTrigger("OpenCardOptions");
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
