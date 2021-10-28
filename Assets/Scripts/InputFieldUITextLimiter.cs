using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldUITextLimiter : MonoBehaviour
{
    [SerializeField]
    InputField inputField;

    private void OnEnable()
    {
        inputField.characterLimit = 6;
    }
}
