//Tyson Shannon 2026-03-31

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiTrailerSpeed : IDrivingSpeedStrategy
{
    public float GetDriveSpeed()
    {
        return 0.4f;
    }

}

public class SemiTrailerSteer : IDrivingSteerStrategy
{
    public float GetDriveSteer()
    {
        return 0.45f;
    }

}
