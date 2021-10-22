using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardNumberText
{
    [Range(0, 1)]
    public float textX;
    [Range(0, 1)]
    public float textY;
    public string cardText;
    public Color textColor;
}
