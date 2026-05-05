using UnityEngine;

public class RoamingState : IZombieState
{
    public void Enter(ZombieAttacher zombie)
    {
        zombie.DetachFromCar();
        zombie.EnablePhysics(true);
    }

    public void UpdateState(ZombieAttacher zombie)
    {
        zombie.MoveTowardCar();

        if (zombie.CanTryAttach())
        {
            zombie.TryAttachToCar();
        }
    }

    public void HandleCollision(ZombieAttacher zombie, Collision collision)
    {
        zombie.TryAttachFromCollider(collision.collider);
    }

    public void Exit(ZombieAttacher zombie)
    {
    }
}

public class AttachedState : IZombieState
{
    public void Enter(ZombieAttacher zombie)
    {
        zombie.EnablePhysics(false);
        zombie.ResetDamageTimer();
    }

    public void UpdateState(ZombieAttacher zombie)
    {
        if (!zombie.IsAttached)
        {
            zombie.ChangeState(new RoamingState());
            return;
        }

        zombie.FollowAttachPoint();
        zombie.UpdateAttachedDamage();
    }

    public void HandleCollision(ZombieAttacher zombie, Collision collision)
    {
    }

    public void Exit(ZombieAttacher zombie)
    {
        zombie.ResetDamageTimer();
    }
}

public class DeadState : IZombieState
{
    public void Enter(ZombieAttacher zombie)
    {
        zombie.DetachFromCar();
        zombie.EnablePhysics(false);
        zombie.ReturnToPoolOrDestroy();
    }

    public void UpdateState(ZombieAttacher zombie)
    {
    }

    public void HandleCollision(ZombieAttacher zombie, Collision collision)
    {
    }

    public void Exit(ZombieAttacher zombie)
    {
    }
}
