using UnityEngine;

//attach to ZombieSystem object
//spawns basic zombies from the pool
public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private ZombiePool zombiePool;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Timing")]
    [SerializeField] private float startSpawnInterval = 4f;
    [SerializeField] private float minimumSpawnInterval = 1f;
    [SerializeField] private float difficultyIncreasePerSecond = 0.03f;

    private float spawnTimer;
    private float currentSpawnInterval;
    private bool canSpawn = true;

    private void Awake()
    {
        if (zombiePool == null)
        {
            zombiePool = GetComponent<ZombiePool>();
        }

        currentSpawnInterval = startSpawnInterval;
    }

    private void OnEnable()
    {
        ZombieEvents.OnGameOver += StopSpawning;
        ZombieEvents.OnCarDestroyed += StopSpawning;
        CarHealthManager.OnCarDestroyed += StopSpawningFromHealthManager;
    }

    private void OnDisable()
    {
        ZombieEvents.OnGameOver -= StopSpawning;
        ZombieEvents.OnCarDestroyed -= StopSpawning;
        CarHealthManager.OnCarDestroyed -= StopSpawningFromHealthManager;
    }

    private void Update()
    {
        if (!canSpawn || zombiePool == null || spawnPoints.Length == 0)
        {
            return;
        }

        currentSpawnInterval = Mathf.Max(
            minimumSpawnInterval,
            currentSpawnInterval - difficultyIncreasePerSecond * Time.deltaTime
        );

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }
    }

    public void SpawnZombie()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null)
        {
            return;
        }

        zombiePool.GetZombie(ZombieType.Basic, spawnPoint.position, spawnPoint.rotation);
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
        {
            return null;
        }

        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }

    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void StopSpawningFromHealthManager(CarHealthManager.DamageSource source)
    {
        StopSpawning();
    }
}
