//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : Pool<PowerUp, PowerUp.PowerUpType>
{
    private Stack<PowerUp> poolHealthPower = new Stack<PowerUp>();
    private Stack<PowerUp> poolSpeedPower = new Stack<PowerUp>();
    private Stack<PowerUp> poolRepelPower = new Stack<PowerUp>();
    private Stack<PowerUp> poolShieldPower = new Stack<PowerUp>();
    private Stack<PowerUp> poolMultiPower = new Stack<PowerUp>();

    protected override Stack<PowerUp> ResolvePool(PowerUp.PowerUpType type){
        switch (type) {
            case PowerUp.PowerUpType.Health: return poolHealthPower;
            case PowerUp.PowerUpType.Speed: return poolSpeedPower;
            case PowerUp.PowerUpType.Repel: return poolRepelPower;
            case PowerUp.PowerUpType.Shield: return poolShieldPower;
            case PowerUp.PowerUpType.Multi: return poolMultiPower;
        }
        return null;
    }

    protected override string ResolvePath(PowerUp.PowerUpType type){
        const string prefix = "Prefab/PowerUps/";
        switch (type) {
            case PowerUp.PowerUpType.Health: return prefix + "healthPower"; 
            case PowerUp.PowerUpType.Speed: return prefix + "speedPower";
            case PowerUp.PowerUpType.Repel: return prefix + "repelPower";
            case PowerUp.PowerUpType.Shield: return prefix + "shieldPower";
            case PowerUp.PowerUpType.Multi: return prefix + "multiPower";
        }

        return null;
    }

    public static PowerUpPool CreatePowerupPool(int poolMax){
        var selfObj = new GameObject("PowerUpPool");
        var self = selfObj.AddComponent<PowerUpPool>();
        
        Pool<PowerUp, PowerUp.PowerUpType>.CreatePool(self, poolMax);

        return self;
    }
}