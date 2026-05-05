using System.Collections.Generic;
using UnityEngine;

//attach to player car
//tracks attach points and applies zombie damage
public class ZombieDamageManager : MonoBehaviour
{
    [SerializeField] private Transform[] attachPoints;
    [SerializeField] private CarHealthManager carHealthManager;

    private readonly Dictionary<Transform, ZombieAttacher> occupiedPoints = new Dictionary<Transform, ZombieAttacher>();
    private readonly Dictionary<ZombieAttacher, Transform> zombiePoints = new Dictionary<ZombieAttacher, Transform>();

    private void Awake()
    {
        if (carHealthManager == null)
        {
            carHealthManager = GetComponent<CarHealthManager>();
        }

        if (carHealthManager == null)
        {
            carHealthManager = GetComponentInParent<CarHealthManager>();
        }

        if (carHealthManager == null)
        {
            carHealthManager = GetComponentInChildren<CarHealthManager>();
        }
    }

    public Transform GetClosestAvailableAttachPoint(Vector3 zombiePosition)
    {
        Transform closestPoint = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < attachPoints.Length; i++)
        {
            Transform point = attachPoints[i];
            if (point == null || occupiedPoints.ContainsKey(point))
            {
                continue;
            }

            float distance = Vector3.Distance(zombiePosition, point.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    public bool RegisterZombie(ZombieAttacher zombie, Transform attachPoint)
    {
        if (zombie == null || attachPoint == null)
        {
            return false;
        }

        if (occupiedPoints.ContainsKey(attachPoint))
        {
            return false;
        }

        if (zombiePoints.ContainsKey(zombie))
        {
            UnregisterZombie(zombie);
        }

        occupiedPoints.Add(attachPoint, zombie);
        zombiePoints.Add(zombie, attachPoint);
        return true;
    }

    public void UnregisterZombie(ZombieAttacher zombie)
    {
        if (zombie == null || !zombiePoints.TryGetValue(zombie, out Transform attachPoint))
        {
            return;
        }

        zombiePoints.Remove(zombie);

        if (occupiedPoints.TryGetValue(attachPoint, out ZombieAttacher attachedZombie) && attachedZombie == zombie)
        {
            occupiedPoints.Remove(attachPoint);
        }
    }

    public void DamageCar(int damage)
    {
        if (carHealthManager != null)
        {
            carHealthManager.TakeDamage(damage, CarHealthManager.DamageSource.Zombie);
        }
    }

    public bool HasAvailableAttachPoint()
    {
        return GetAvailableAttachPointCount() > 0;
    }

    public int GetAvailableAttachPointCount()
    {
        int count = 0;

        for (int i = 0; i < attachPoints.Length; i++)
        {
            Transform point = attachPoints[i];
            if (point != null && !occupiedPoints.ContainsKey(point))
            {
                count++;
            }
        }

        return count;
    }

    public bool HasAttachedZombies()
    {
        return zombiePoints.Count > 0;
    }

    public int GetAttachedZombieCount()
    {
        return zombiePoints.Count;
    }

    public void DamageAttachedZombies(int damage)
    {
        List<ZombieAttacher> zombiesToDamage = new List<ZombieAttacher>(zombiePoints.Keys);

        for (int i = 0; i < zombiesToDamage.Count; i++)
        {
            ZombieAttacher zombie = zombiesToDamage[i];

            if (zombie != null && zombie.IsAttached)
            {
                zombie.TakeDamage(damage);
            }
        }
    }
}
