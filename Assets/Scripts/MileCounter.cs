//Tyson Shannon (original) 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MileCounter : MonoBehaviour
{
    private float milesTraveled;
    private float carSpeed;
    [SerializeField] private CarController car;

    void Start()
    {
        milesTraveled = 0f;
    }

    void Update()
    {
        carSpeed = car.GetSpeed();
        milesTraveled += carSpeed;
        Debug.Log("Miles: " + milesTraveled);
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddDistance(carSpeed, true);
    }
}