//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//attach to healthPower prefab
//determines if player collides and adds health before releasing object to pool
public class HealthUpCollide : PowerUp
{
    [SerializeField] protected CarHealthManager carHealth;

    public override void AttachSceneObjects(){
        base.AttachSceneObjects(); // Need to attach the car.
        
        if (carHealth == null) {
            carHealth = FindObjectOfType<CarHealthManager>(); // Also need to attach the CarHealthManager.
        }
    }

    protected override void PowerUp_OnCollisionEffects(Collider other){
        carHealth.Repair(30); // We should make this a serialized field.
    }

    public class HealthCollideFactory : PowerUpFactory{
        public override SpawningEvent CreateSpawningEvent(Vector3 position,  Quaternion rotation){
            var self = (HealthUpCollide)base.pool.Get(PowerUp.PowerUpType.Health, position, rotation); // Get the object
            self.pool = base.pool; // Give the object its pool.
            return self; // Return the object
        }
    }
}
