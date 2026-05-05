using System;

public static class ZombieEvents
{
    public static event Action OnGameOver;
    public static event Action OnCarDestroyed;
    public static event Action<ZombieAttacher> OnZombieKilled;
    public static event Action<ZombieAttacher> OnZombieSpawned;
    public static event Action<int> OnZombieDamagePowerUp;

    public static void RaiseGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void RaiseCarDestroyed()
    {
        OnCarDestroyed?.Invoke();
    }

    public static void RaiseZombieKilled(ZombieAttacher zombie)
    {
        OnZombieKilled?.Invoke(zombie);
    }

    public static void RaiseZombieSpawned(ZombieAttacher zombie)
    {
        OnZombieSpawned?.Invoke(zombie);
    }

    public static void TriggerZombieDamagePowerUp(int damage)
    {
        OnZombieDamagePowerUp?.Invoke(damage);
    }
}
