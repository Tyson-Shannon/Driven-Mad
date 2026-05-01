//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach to speedup prefab
//determines if player collides and adds boost before releasing object to pool
public class SpeedUpCollide : PowerUp
{
    [SerializeField] private float boostAmount = 1f;
    [SerializeField] private float boostDuration = 5f;

    protected override void PowerUp_OnCollisionEffects(Collider other){
        car.ApplySpeedUp(boostAmount, boostDuration);
    }

    public class SpeedUpCollideFactory : PowerUpFactory {
        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            SpeedUpCollide self =
                base.pool.Get(PowerUp.PowerUpType.Speed, position, rotation)
                as SpeedUpCollide;
            self.pool = base.pool;
            return self;
        }
    }
}
