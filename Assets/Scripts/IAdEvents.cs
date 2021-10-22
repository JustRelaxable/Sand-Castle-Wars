using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdEvents
{
    public event Action OnAdsReady;
    public event Action OnAdsSuccessfullyWatched;
}
