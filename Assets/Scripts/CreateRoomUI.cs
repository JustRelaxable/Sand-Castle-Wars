using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private InputField roomNameField;

    [SerializeField]
    private SimpleMatchMaker simpleMatchMaker;


    public void OnCreateButtonClicked()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
            return;
        simpleMatchMaker.CreateInternetMatch(roomNameField.text,this);
    }
}
