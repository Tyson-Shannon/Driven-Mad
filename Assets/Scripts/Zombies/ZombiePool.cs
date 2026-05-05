using System.Collections.Generic;
using UnityEngine;

//keeps zombies ready so gameplay does not destroy them
public class ZombiePool : MonoBehaviour
{
    [SerializeField] private ZombieFactory factory;
    [SerializeField] private bool allowPoolGrowth = true;

    private readonly Dictionary<ZombieType, Queue<ZombieAttacher>> pools = new Dictionary<ZombieType, Queue<ZombieAttacher>>();

    private void Awake()
    {
        if (factory == null)
        {
            factory = GetComponent<ZombieFactory>();
        }

        BuildInitialPools();
    }

    public ZombieAttacher GetZombie(ZombieType type, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(type))
        {
            pools.Add(type, new Queue<ZombieAttacher>());
        }

        ZombieAttacher zombie = null;

        if (pools[type].Count > 0)
        {
            zombie = pools[type].Dequeue();
        }
        else if (allowPoolGrowth && factory != null)
        {
            zombie = factory.CreateZombie(type, transform);
            if (zombie != null)
            {
                zombie.SetPool(this);
            }
        }

        if (zombie == null)
        {
            return null;
        }

        zombie.transform.SetParent(null);
        zombie.transform.position = position;
        zombie.transform.rotation = rotation;
        zombie.gameObject.SetActive(true);
        zombie.ResetZombie();
        ZombieEvents.RaiseZombieSpawned(zombie);
        return zombie;
    }

    public void ReturnZombie(ZombieAttacher zombie)
    {
        if (zombie == null)
        {
            return;
        }

        Rigidbody rb = zombie.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        zombie.DetachFromCar();
        zombie.transform.SetParent(transform);
        zombie.gameObject.SetActive(false);

        ZombieType type = zombie.Type;
        if (!pools.ContainsKey(type))
        {
            pools.Add(type, new Queue<ZombieAttacher>());
        }

        pools[type].Enqueue(zombie);
    }

    private void BuildInitialPools()
    {
        if (factory == null)
        {
            Debug.LogWarning("ZombiePool needs a ZombieFactory.");
            return;
        }

        ZombieData[] allData = factory.GetAllZombieData();
        for (int i = 0; i < allData.Length; i++)
        {
            ZombieData data = allData[i];
            if (data == null)
            {
                continue;
            }

            if (!pools.ContainsKey(data.type))
            {
                pools.Add(data.type, new Queue<ZombieAttacher>());
            }

            for (int j = 0; j < data.initialPoolSize; j++)
            {
                ZombieAttacher zombie = factory.CreateZombie(data.type, transform);
                if (zombie == null)
                {
                    continue;
                }

                zombie.SetPool(this);
                pools[data.type].Enqueue(zombie);
            }
        }
    }
}
