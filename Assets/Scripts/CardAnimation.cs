using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private Animator animator;
    public bool onSelf = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        var animationClip = animator.runtimeAnimatorController.animationClips[0];
        var animationEvent = new AnimationEvent();
        animationEvent.functionName = "DestroyTheGameObject";
        animationEvent.time = animationClip.length;
        animationClip.AddEvent(animationEvent);
        StartAction();
    }
    public virtual void StartAction()
    {

    }
    public void DestroyTheGameObject()
    {
        Destroy(this.gameObject);
    }
}
