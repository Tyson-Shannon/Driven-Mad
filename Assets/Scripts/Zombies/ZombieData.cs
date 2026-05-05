using UnityEngine;

[System.Serializable]
public class ZombieData
{
    public ZombieType type;
    public GameObject prefab;
    public int maxHP = 100;
    public int damagePerTick = 5;
    public float damageInterval = 2f;
    public float roamSpeed = 1f;
    public float detectRange = 8f;
    public int initialPoolSize = 5;
}
