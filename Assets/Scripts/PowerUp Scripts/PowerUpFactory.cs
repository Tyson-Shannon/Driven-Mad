//Tyson Shannon 2026-04-06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PowerUpFactory : MonoBehaviour
//{
//    [SerializeField] private PowerUpPool pool;
//    private List<GameObject> activePowerUps = new List<GameObject>();
//
//    //create/retrieve powerup in pool and spawn it
//    public GameObject CreatePowerUp(PowerUp.PowerUpType type, Vector3 position)
//    {
//        GameObject obj = pool.Get(type);
//        obj.transform.position = position;
//        obj.transform.rotation = Quaternion.identity;
//        obj.SetActive(true);
//        activePowerUps.Add(obj);
//        return obj;
//    }
//    //destroy/return powerup in pool
//    public void ReleasePowerUp(GameObject obj)//powerups should call this when they die
//    {
//        if (activePowerUps.Contains(obj))
//        {
//            pool.Release(obj);
//            activePowerUps.Remove(obj);
//        }
//    }
//}
