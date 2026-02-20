//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCarSpeed : IDrivingSpeedStrategy
{
    public float GetDriveSpeed()
    {
        return 0.4f;
    }

}

public class BasicCarSteer : IDrivingSteerStrategy
{
    public float GetDriveSteer()
    {
        return 2.0f;
    }

}
