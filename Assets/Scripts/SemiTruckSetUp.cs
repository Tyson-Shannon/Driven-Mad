//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiTruckSetUp : MonoBehaviour
{
    private void Awake()
    {
        CarController car = GetComponent<CarController>();

        IDrivingSpeedStrategy driveSpeed = new SemiTruckSpeed();
        IDrivingSteerStrategy driveSteer = new SemiTruckSteer();

        car.Initialize(driveSpeed, driveSteer);
    }
}
