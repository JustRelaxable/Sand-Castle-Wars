using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicatorUI : MonoBehaviour
{
    private Text indicatorText;
    private void Awake()
    {
        indicatorText = GetComponent<Text>();
    }
    public void SetIndicatorText(bool isMyTurn)
    {
        indicatorText.text = isMyTurn ? "Your Turn" : "Opponent Turn";
    }
    public void SetIndicatorText(string text)
    {
        indicatorText.text = text;
    }
}
