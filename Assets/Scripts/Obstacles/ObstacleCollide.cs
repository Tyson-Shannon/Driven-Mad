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
        transform.Translate(new Vector3(0, 0, -(carSpeed * Time.deltaTime * 10)));
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
        private PgcSingleton _rng = PgcSingleton.CreatePgcSingleton();

        public ObstacleCollideFactory(){
            base.pool = ObstaclePool.CreateObstaclePool(10);
        }

        public override unsafe SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            PrimitiveUnion roll = _rng.RandomPrimativeUnion();
            ObstacleType prefabType = (roll._valueByte[0] < sbyte.MaxValue) 
                ? ObstacleType.LeftPole 
                : ObstacleType.RightPole;
            
            ObstacleCollide obstacle = base.pool.Get(prefabType, position, rotation);
            obstacle.SetupObstacleFactory();
            
            return obstacle;
        }
    }
}
