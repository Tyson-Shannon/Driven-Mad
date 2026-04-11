using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private PowerUpFactory factory;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            factory.CreatePowerUp(PowerUpType.Shield, new Vector3(1.51f, -4.5f, 24f));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            factory.CreatePowerUp(PowerUpType.Health, new Vector3(1.51f, -4.5f, 24f));
        }

    }
}
