using UnityEngine;

#region NON_GENERIC
public abstract class SpawningEvent : MonoBehaviour {}
#endregion NON_GENERIC

public abstract class SpawningEvent<T, U> : SpawningEvent
    where T : SpawningEvent
    where U : System.Enum {
  protected Pool<T,  U> pool;
  public U type;
  
  private void Despawn(){
    pool.Release((T)this);
  }
  protected static void CreateAbstractSpawningEvent(SpawningEvent<T, U> spawnableEvent, U type){
    spawnableEvent.type = type;
  }

  private void OnTriggerEnter(Collider other){
     if (other.CompareTag("ObjectCatcher")) {
       this.Despawn();
       return;
     }
     
     this.OnCollisionTemplate(other);
  }

  private void OnCollisionTemplate(Collider other){
    if (this.OnCollisionCondition(other)) {
      OnCollisionEffects(other);
      this.Despawn();
    }
  }
  
  protected virtual void OnCollisionEffects(Collider other){}

  protected virtual bool OnCollisionCondition(Collider other){
    return other.CompareTag("Player");
  }
}
