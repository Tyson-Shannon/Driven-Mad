//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollide : SpawningEvent<ObstacleCollide, ObstacleCollide.ObstacleType>
{
    public enum ObstacleType
    {
        LeftPole,
        RightPole
    }

    // Used for enabling/disabling.
    GameObject obstacle;
    //[SerializeField] private ObstacleFactory factory;
    
    [SerializeField] ObstacleType type;

    [SerializeField] private CarController car;
    private float carSpeed;

    [SerializeField] private DeathFacade deathFacade;

    public override void AttachSceneObjects(){
        if (car == null) {
            car = FindObjectOfType<CarController>();
        }

        if (deathFacade == null) {
            deathFacade = FindObjectOfType<DeathFacade>();
        }
    }
    
    private void Update()
    {
        //move obstacle to look like car drives towards it
        carSpeed = car.GetSpeed();
        transform.Translate(new Vector3(0, 0, -(carSpeed * Time.deltaTime * 10)), Space.World);
    }

    protected override void OnCollisionEffects(Collider other){
        if (car != null)
        {
            //end game
            Debug.Log("Car Crash");
            deathFacade.Die();
        }
    }

    private void SetupObstacleFactory(){
        //Any additional setup stuff goes here. Call it in the factory's create method.
    }

    public class ObstacleCollideFactory : EventFactoryNegative<ObstacleCollide, ObstacleCollide.ObstacleType> {
        private PgcSingleton _rng = PgcSingleton.CreatePgcSingleton(); // Because left and right have the same type
        // (class, not enum), we can't assign a factory for each. Thus, we determine the type in the factory class.

        public ObstacleCollideFactory(){
            base.pool = ObstaclePool.CreateObstaclePool(10);
        }

        public override unsafe SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            PrimitiveUnion roll = _rng.RandomPrimativeUnion(); // Roll for a type.
            float laneOffset = 8.5f; // Positions poles to the sides.
            
            // Determine which pole we're making.
            ObstacleType prefabType;
            if (roll._valueByte[0] < sbyte.MaxValue) {
                prefabType = ObstacleType.LeftPole;
                laneOffset *= -1;
            }
            else {
                prefabType = ObstacleType.RightPole;
            }

            ObstacleCollide obstacle = base.pool.Get(prefabType, position, rotation);
            obstacle.transform.position = new Vector3(position.x + laneOffset, position.y, position.z); // Give the object
            // An offset from the spawner.
            obstacle.SetupObstacleFactory();
            obstacle.pool = this.pool;
            
            return obstacle;
        }
    }
}
