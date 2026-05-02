//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp, IPowerUpVisitor
{
    private PowerUpType type = PowerUpType.Shield;
    private int shieldAmount = 50;

    public void Visit(CarHealthManager car)
    {
        car.ActivateShield(shieldAmount);

        Debug.Log("Shield powerup applied via Visitor pattern");
    }

    protected override void PowerUp_OnCollisionEffects(Collider other){
        CarHealthManager carHealth = other.GetComponent<CarHealthManager>();

        if (carHealth != null)
        {
            carHealth.Accept(this);
        }
    }

    public class ShieldUpVisitorFactory : PowerUpFactory {
        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            var self = (ShieldPowerUp)base.pool.Get(PowerUp.PowerUpType.Shield, position, rotation);
            self.pool = base.pool;
            return self;
        }
    }
}
