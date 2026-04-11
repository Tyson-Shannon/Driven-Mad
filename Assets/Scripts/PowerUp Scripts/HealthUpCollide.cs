//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//attach to healthPower prefab
//determines if player collides and adds health before releasing object to pool
public class HealthUpCollide : MonoBehaviour
{
    [SerializeField] private PowerUpFactory factory;

    [SerializeField] private CarController car;
    private float carSpeed;

    [SerializeField] private CarHealthManager carHealth;

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
                //heal car
                carHealth.Repair(30);
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
