using UnityEngine;

//creates configured zombie objects for the pool
public class ZombieFactory : MonoBehaviour
{
    [SerializeField] private ZombieData[] zombieConfigs;

    public ZombieAttacher CreateZombie(ZombieType type, Transform parent)
    {
        ZombieData data = GetZombieData(type);
        if (data == null || data.prefab == null)
        {
            Debug.LogWarning("Missing zombie config or prefab for " + type);
            return null;
        }

        GameObject zombieObject = Instantiate(data.prefab, parent);
        zombieObject.SetActive(false);

        ZombieAttacher zombie = zombieObject.GetComponent<ZombieAttacher>();
        if (zombie == null)
        {
            zombie = zombieObject.AddComponent<ZombieAttacher>();
        }

        zombie.Configure(data);
        return zombie;
    }

    public ZombieData GetZombieData(ZombieType type)
    {
        for (int i = 0; i < zombieConfigs.Length; i++)
        {
            if (zombieConfigs[i] != null && zombieConfigs[i].type == type)
            {
                return zombieConfigs[i];
            }
        }

        return null;
    }

    public ZombieData[] GetAllZombieData()
    {
        return zombieConfigs;
    }
}
