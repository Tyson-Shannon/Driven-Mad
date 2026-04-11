//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//attach to speedup prefab
//determines if player collides and adds boost before releasing object to pool
public class SpeedUpCollide : MonoBehaviour
{
    [SerializeField] private PowerUpFactory factory;

    [SerializeField] private float boostAmount = 1f;
    [SerializeField] private float boostDuration = 5f;

    [SerializeField] private CarController car;
    private float carSpeed;

    private bool isCollected = false;//makes sure powerup isn't triggered more than once per life

    private void OnEnable()
    {
        isCollected = false;
    }

    private void Update()
    {
        //move powerup to look like car drives towards it
        carSpeed = car.GetSpeed();
        transform.Translate(new Vector3(0, 0, -(carSpeed * Time.deltaTime * 10)));
    }

    private void OnTriggerEnter(Collider other)
    {
        //if car collides with powerup
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true; //mark as used immediately

            if (car != null)
            {
                car.ApplySpeedUp(boostAmount, boostDuration);
            }

            if (factory != null)
            {
                factory.ReleasePowerUp(gameObject);
            }
        }
        //if powerup passes car it will eventually hit catcher and be released
        if (other.CompareTag("ObjectCatcher"))
        {
            if (factory != null)
            {
                factory.ReleasePowerUp(gameObject);
            }
        }
    }
}
