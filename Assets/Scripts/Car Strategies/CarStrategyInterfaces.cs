//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrivingSpeedStrategy
{
    float GetDriveSpeed();
}

public interface IDrivingSteerStrategy
{
    float GetDriveSteer();
}
