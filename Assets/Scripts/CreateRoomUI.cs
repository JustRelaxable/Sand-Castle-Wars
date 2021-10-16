using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private InputField roomNameField;

    [SerializeField]
    private InputField roomPasswordField;

    [SerializeField]
    private SimpleMatchMaker simpleMatchMaker;

    [SerializeField]
    private PhotonSimpleMatchMaker photonSimpleMatchMaker;


    public void OnCreateButtonClicked()
    {

        if (string.IsNullOrEmpty(roomNameField.text))
            return;
        photonSimpleMatchMaker.CreateInternetMatch(roomNameField.text);
        //simpleMatchMaker.CreateInternetMatch(roomNameField.text,this, roomPasswordField.text);
    }
}
