using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayedCardUI : MonoBehaviour
{
    [SerializeField]
    float zFactor;

    public Vector3 GetNewCardLocalPosition()
    {
        var firstChildTransform = transform.GetChild(0);
        var zVector = new Vector3(0, 0, zFactor);
        if (transform.childCount == 1)
            return firstChildTransform.localPosition + zVector;
        else if (transform.childCount == 2)
            return firstChildTransform.localPosition + 2*zVector;
        else
        {
            transform.GetChild(2).transform.localPosition = firstChildTransform.localPosition + zVector;
            Destroy(transform.GetChild(1).gameObject);
            return firstChildTransform.localPosition + 2*zVector;
        }
    }
}
