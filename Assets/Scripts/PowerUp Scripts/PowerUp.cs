using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : SpawningEvent<PowerUp, PowerUp.PowerUpType> {
  [SerializeField] protected CarController car;
  protected float carSpeed;
  private bool isCollected;

  public override void AttachSceneObjects(){ // Every powerup needs to attach the car controller.
    if (car == null) {
      car = FindObjectOfType<CarController>(); // Attach stuff from the scene.
    }
  }

  public enum PowerUpType {
    Health,
    Speed,
    Repel,
    Shield,
    Multi
  }

  private void OnEnable(){
    isCollected = false;
  }

  private void Update(){
    PowerUp_Update();
  }

  protected virtual void PowerUp_Update(){
    //move powerup to look like car drives towards it
    carSpeed = car.GetSpeed();
    transform.Translate(new Vector3(0, 0, -(carSpeed * Time.deltaTime * 10)));
  }

  protected static void CreatePowerUp(PowerUp pU, PowerUp.PowerUpType type){ // For use in concrete constructors.
    SpawningEvent<PowerUp, PowerUp.PowerUpType>.CreateAbstractSpawningEvent(pU, type);
  }

  protected sealed override bool OnCollisionCondition(Collider other){ // Business rule for determining powerup hits.
    return !this.isCollected && other.CompareTag("Player");
  }

  protected sealed override void OnCollisionEffects(Collider other){ // Business rule for what to do when the car hits a powerup.
    this.isCollected = true;
    if (car != null) {
      this.PowerUp_OnCollisionEffects(other);
    }
  }

  protected abstract void PowerUp_OnCollisionEffects(Collider other); // What the subclasses do when they hit.

  public abstract class PowerUpFactory : EventFactoryPositive<PowerUp, PowerUp.PowerUpType> { // The factory shares the
    // pool with the GameObjects. The factories will also require the pool for object construction.
    protected PowerUpFactory(){
      base.pool = PowerUpPool.CreatePowerupPool(10);
    }
  }
}
