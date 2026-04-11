//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour, IPowerUpVisitor
{
    private int shieldAmount = 50;
    [SerializeField] private PowerUpFactory factory;
    [SerializeField] private CarController car;
    private float carSpeed;

    public void Visit(CarHealthManager car)
    {
        car.ActivateShield(shieldAmount);

        Debug.Log("Shield powerup applied via Visitor pattern");
    }

    private void Update()
    {
        //move powerup to look like car drives towards it
        carSpeed = car.GetSpeed();
        transform.Translate(new Vector3(0, 0, -(carSpeed * Time.deltaTime * 10)));
    }

    private void OnTriggerEnter(Collider other)
    {
        CarHealthManager carHealth = other.GetComponent<CarHealthManager>();

        if (carHealth != null)
        {
            carHealth.Accept(this);
            factory.ReleasePowerUp(gameObject);
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
