using UnityEngine;

#region NON_GENERIC

public abstract class SpawningEvent : MonoBehaviour { // Non-Generic to simplify the other parts of the codebase.
  public abstract System.Enum EventType { get; }
  public abstract void AttachSceneObjects(); // How to get Live Scene objects attached to serialized fields.
}
#endregion NON_GENERIC

public abstract class SpawningEvent<T, U> : SpawningEvent
    where T : SpawningEvent
    where U : System.Enum {
  protected Pool<T,  U> pool; // Don't actually do anything with this, it's meant for despawnning.
  private U type;
  public sealed override System.Enum EventType => type;
  
  private void Despawn(){
    pool.Release((T)(object)this);
  }
  
  protected static void CreateAbstractSpawningEvent(SpawningEvent<T, U> spawnableEvent, U type){ // Used in constructors.
    spawnableEvent.type = type;
  }

  private void OnTriggerEnter(Collider other){
     if (other.CompareTag("ObjectCatcher")) { // Did the object hit the object catcher?
       Debug.Log("ObjectCatcher hit: " + this.gameObject.name);
       this.Despawn();
       return;
     }
     
     this.OnCollisionTemplate(other); // What to do if it hits the player.
  }

  private void OnCollisionTemplate(Collider other){
    if (this.OnCollisionCondition(other)) { // What determines if the player hit the object.
      this.OnCollisionEffects(other); // What to do if the player hits the object.
      this.Despawn();
    }
  }
  
  protected virtual void OnCollisionEffects(Collider other){}

  protected virtual bool OnCollisionCondition(Collider other){
    return other.CompareTag("Player");
  }
}
