using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
//    [SerializeField] private PowerUpFactory powerFactory;
//    [SerializeField] private ObstacleFactory obstacleFactory;
//    //timer set to zero
//    private float powerTimer = 0f;
//    private float obstacleTimer = 0f;
//    //delay before spawn
//    private float powerDelay = 5f;
//    private float obstacleDelay = 6f;
//
//    private List<PowerUpType> powerUpTypes = new List<PowerUpType>();
//    private List<ObstacleType> obstacleTypes = new List<ObstacleType>();
//
//    private int powerIndex;
//    private int obstacleIndex;
//
//    void Start()
//    {
//        powerUpTypes.Add(PowerUpType.Health);
//        powerUpTypes.Add(PowerUpType.Shield);
//        powerUpTypes.Add(PowerUpType.Speed);
//
//        obstacleTypes.Add(ObstacleType.LeftPole);
//        obstacleTypes.Add(ObstacleType.RightPole);
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        powerTimer += Time.deltaTime;
//        obstacleTimer += Time.deltaTime;
//        //get random index
//        powerIndex = Random.Range(0, powerUpTypes.Count);
//        obstacleIndex = Random.Range(0, obstacleTypes.Count);
//        //specified time has passed to spawn random powerup
//        if (powerTimer >= powerDelay)
//        {
//            powerTimer = 0f;
//            powerFactory.CreatePowerUp(powerUpTypes[powerIndex], new Vector3(1.51f, -4.5f, 24f));
//        }
//        if (obstacleTimer >= obstacleDelay)
//        {
//            obstacleTimer = 0f;
//            //obstacleFactory.CreateObstacle(obstacleTypes[obstacleIndex], new Vector3(-30f, -0.04f, -6.13f), Quaternion.identity);
//        }
//    }
}
