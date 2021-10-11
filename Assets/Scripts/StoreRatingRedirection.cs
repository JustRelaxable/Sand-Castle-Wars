using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRatingRedirection : MonoBehaviour
{
    public void OnClickRateUs()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
