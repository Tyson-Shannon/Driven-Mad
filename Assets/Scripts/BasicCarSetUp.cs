//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCarSetUp : MonoBehaviour
{
    private void Awake()
    {
        CarController car = GetComponent<CarController>();

        IDrivingSpeedStrategy driveSpeed = new BasicCarSpeed();
        IDrivingSteerStrategy driveSteer = new BasicCarSteer();

        car.Initialize(driveSpeed, driveSteer);

        // Barry - auto attach health manager
        if (GetComponent<CarHealthManager>() == null)
        {
            gameObject.AddComponent<CarHealthManager>();
        }
    }
}
