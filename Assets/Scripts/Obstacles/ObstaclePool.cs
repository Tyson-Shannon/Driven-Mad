//Tyson Shannon 2026-04-11

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    private int poolMax = 10;

    [SerializeField] private GameObject leftPole;
    [SerializeField] private GameObject rightPole;

    public GameObject Get(ObstacleType type)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf && MatchesType(obj, type))
            {
                return obj;
            }
        }
        //create new if not found
        GameObject prefab = GetPrefab(type);
        GameObject obstacle = Instantiate(prefab);


        // add to pool if room
        if (pool.Count < poolMax)
        {
            pool.Add(obstacle);
        }

        return obstacle;
    }

    public void Release(GameObject obj)
    {
        //return to pool by setting it inactive or destroy
        if (pool.Contains(obj))
        {
            obj.SetActive(false);
        }
        else
        {
            Destroy(obj);
        }
    }

    private GameObject GetPrefab(ObstacleType type)
    {
        switch (type)
        {
            case ObstacleType.LeftPole: return leftPole;
            case ObstacleType.RightPole: return rightPole;
        }
        return null;
    }

    private bool MatchesType(GameObject obj, ObstacleType type)
    {
        return obj.name.Contains(type.ToString().ToLower());
    }
}