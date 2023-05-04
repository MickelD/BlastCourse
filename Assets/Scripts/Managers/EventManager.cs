using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    #region Events

    //Player Events
    public static Action<float> OnUpdatePlayerSpeedXYZ;
    public static Action<float> OnUpdatePlayerSpeedXZ;
    public static Action<float> OnUpdatePlayerSpeedY;
    public static Action<float> OnPlayerLanded;

    #endregion
}
