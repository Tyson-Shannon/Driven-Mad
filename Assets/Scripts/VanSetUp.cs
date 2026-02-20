//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanSetUp : MonoBehaviour
{
    private void Awake()
    {
        CarController car = GetComponent<CarController>();

        IDrivingSpeedStrategy driveSpeed = new VanSpeed();
        IDrivingSteerStrategy driveSteer = new VanSteer();

        car.Initialize(driveSpeed, driveSteer);
    }
}
