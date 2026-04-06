//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpDecorator : IDrivingSpeedStrategy
{
    private IDrivingSpeedStrategy baseStrategy;
    private float boostAmount;

    public SpeedUpDecorator(IDrivingSpeedStrategy baseStrategy, float boostAmount)
    {
        this.baseStrategy = baseStrategy;
        this.boostAmount = boostAmount;
    }

    public float GetDriveSpeed()
    {
        return baseStrategy.GetDriveSpeed() + boostAmount;
    }
}
