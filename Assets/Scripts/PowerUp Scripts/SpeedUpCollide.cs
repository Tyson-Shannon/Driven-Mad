//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach to speedup prefab
//determines if player collides and adds boost before releasing object to pool
public class SpeedUpCollide : MonoBehaviour
{
    [SerializeField] private PowerUpFactory factory;

    [SerializeField] private float boostAmount = 1f;
    [SerializeField] private float boostDuration = 5f;

    private bool isCollected = false;//makes sure powerup isn't triggered more than once per life

    private void OnEnable()
    {
        isCollected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true; // Mark as used immediately

            CarController car = other.GetComponent<CarController>();
            if (car != null)
            {
                car.ApplySpeedUp(boostAmount, boostDuration);
            }

            if (factory != null)
            {
                factory.ReleasePowerUp(gameObject);
            }
        }
    }
}
