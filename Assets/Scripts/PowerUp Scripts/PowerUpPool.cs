//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    private int poolMax = 10;

    [SerializeField] private GameObject healthPower;
    [SerializeField] private GameObject speedPower;
    [SerializeField] private GameObject repelPower;
    [SerializeField] private GameObject shieldPower;
    [SerializeField] private GameObject multiPower;

    public GameObject Get(PowerUpType type)
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
        GameObject powerUp = Instantiate(prefab);

        
        // add to pool if room
        if (pool.Count < poolMax)
        {
            pool.Add(powerUp);
        }

        return powerUp;
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

    private GameObject GetPrefab(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Health: return healthPower;
            case PowerUpType.Speed: return speedPower;
            case PowerUpType.Repel: return repelPower;
            case PowerUpType.Shield: return shieldPower;
            case PowerUpType.Multi: return multiPower;
        }
        return null;
    }

    private bool MatchesType(GameObject obj, PowerUpType type)
    {
        return obj.name.Contains(type.ToString());
    }
}