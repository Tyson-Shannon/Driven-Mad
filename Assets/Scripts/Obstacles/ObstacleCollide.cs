//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollide : MonoBehaviour
{
    [SerializeField] private ObstacleFactory factory;

    [SerializeField] private CarController car;
    private float carSpeed;

    [SerializeField] private DeathFacade deathFacade;
    private void Update()
    {
        //move obstacle to look like car drives towards it
        carSpeed = car.GetSpeed();
        transform.Translate(new Vector3((carSpeed * Time.deltaTime * 10), 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        //if car collides with obstacle
        if (other.CompareTag("Player"))
        {

            if (car != null)
            {
                //end game
                Debug.Log("Car Crash");
                deathFacade.Die();
            }
        }
        //if obstacle passes car it will eventually hit catcher and be released
        if (other.CompareTag("ObjectCatcher"))
        {
            if (factory != null)
            {
                factory.ReleaseObstacle(gameObject);
            }
        }
    }
}
