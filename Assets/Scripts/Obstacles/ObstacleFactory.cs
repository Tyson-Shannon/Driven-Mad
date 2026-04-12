//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    LeftPole,
    RightPole
}

public class ObstacleFactory : MonoBehaviour
{
    [SerializeField] private ObstaclePool pool;
    private List<GameObject> activeObstacles = new List<GameObject>();

    //create/retrieve obstacle in pool and spawn it
    public GameObject CreateObstacle(ObstacleType type, Vector3 position)
    {
        GameObject obj = pool.Get(type);
        
        obj.SetActive(true);
        activeObstacles.Add(obj);
        return obj;
    }
    //destroy/return obstacle in pool
    public void ReleaseObstacle(GameObject obj)//obstacles should call this when they die
    {
        if (activeObstacles.Contains(obj))
        {
            pool.Release(obj);
            activeObstacles.Remove(obj);
        }
    }
}
