using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private PowerUpFactory factory;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            factory.CreatePowerUp(PowerUpType.Speed, new Vector3(1.51f, -4.5f, 24f));
        }

    }
}
