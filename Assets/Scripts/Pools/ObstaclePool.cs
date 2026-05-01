//Tyson Shannon 2026-04-11

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePool : Pool<ObstacleCollide, ObstacleCollide.ObstacleType>
{
    private readonly Stack<ObstacleCollide> poolLeft = new Stack<ObstacleCollide>();
    private readonly Stack<ObstacleCollide> poolRight = new Stack<ObstacleCollide>();
    
    protected override string ResolvePath(ObstacleCollide.ObstacleType type){
        const string prefix = "Obstacle/";
        switch (type) {
            case ObstacleCollide.ObstacleType.LeftPole: return prefix + "leftPole";
            case ObstacleCollide.ObstacleType.RightPole: return prefix + "rightPole";
        }
        return null;
    }

    protected override Stack<ObstacleCollide> ResolvePool(ObstacleCollide.ObstacleType type){
        switch (type) {
            case ObstacleCollide.ObstacleType.LeftPole: return this.poolLeft;
            case ObstacleCollide.ObstacleType.RightPole: return this.poolRight;
        }
        
        return null;
    }

    public static ObstaclePool CreateObstaclePool(int poolMax){
        var selfObj = new GameObject("ObstaclePool");
        var self = selfObj.AddComponent<ObstaclePool>();
        
        Pool<ObstacleCollide, ObstacleCollide.ObstacleType>.CreatePool(self,  poolMax);
        
        return self;
    }
}