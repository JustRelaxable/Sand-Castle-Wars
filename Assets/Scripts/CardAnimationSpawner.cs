using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationSpawner : MonoBehaviour
{
    public void HandleCardAnimation(int cardID,Teams turn)
    {
        var animationPrefab = CardManager.instance.cards[cardID].animationPrefab;
        if (animationPrefab == null)
            return;

        var animation = Instantiate(animationPrefab, transform);
        var cardAnimation = animation.GetComponent<CardAnimation>();
        if(!cardAnimation.onSelf)
            turn = turn == Teams.Blue ? Teams.Red : Teams.Blue;

        
        //cardAnimation.StartAction();
        if(turn == Teams.Red)
        {
            var r = animation.transform.localRotation;
            animation.transform.localRotation = new Quaternion(r.x, -180, r.z, r.w);
        }
    }
}
