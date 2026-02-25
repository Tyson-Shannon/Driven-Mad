//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleCarSpeed : IDrivingSpeedStrategy
{
    public float GetDriveSpeed()
    {
        return 0.6f;
    }

}

public class MuscleCarSteer : IDrivingSteerStrategy
{
    public float GetDriveSteer()
    {
        return 0.6f;
    }

}
