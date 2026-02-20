//Tyson Shannon 2026-02-19
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleCarSetUp : MonoBehaviour
{
    private void Awake()
    {
        CarController car = GetComponent<CarController>();

        IDrivingSpeedStrategy driveSpeed = new MuscleCarSpeed();
        IDrivingSteerStrategy driveSteer = new MuscleCarSteer();

        car.Initialize(driveSpeed, driveSteer);
    }
}
