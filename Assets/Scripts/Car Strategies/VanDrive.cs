//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanSpeed : IDrivingSpeedStrategy
{
    public float GetDriveSpeed()
    {
        return 0.3f;
    }

}

public class VanSteer : IDrivingSteerStrategy
{
    public float GetDriveSteer()
    {
        return 0.3f;
    }

}
