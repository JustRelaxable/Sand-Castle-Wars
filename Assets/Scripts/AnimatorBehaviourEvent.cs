using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimatorBehaviourEvent : UnityEvent<Animator, AnimatorStateInfo, int>
{
}
