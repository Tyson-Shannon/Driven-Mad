//Tyson Shannon 2026-03-31

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiTrailerSetUp : MonoBehaviour
{
    private void Awake()
    {
        CarController car = GetComponent<CarController>();

        IDrivingSpeedStrategy driveSpeed = new SemiTrailerSpeed();
        IDrivingSteerStrategy driveSteer = new SemiTrailerSteer();

        car.Initialize(driveSpeed, driveSteer);

        // Barry - auto attach health manager
        if (GetComponent<CarHealthManager>() == null)
        {
            gameObject.AddComponent<CarHealthManager>();
        }
    }
}