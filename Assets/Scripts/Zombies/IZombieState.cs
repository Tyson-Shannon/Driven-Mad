using UnityEngine;

public interface IZombieState
{
    void Enter(ZombieAttacher zombie);
    void UpdateState(ZombieAttacher zombie);
    void HandleCollision(ZombieAttacher zombie, Collision collision);
    void Exit(ZombieAttacher zombie);
}
