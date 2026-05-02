
using System.Collections.Generic;
using UnityEngine;


public abstract class Pool<T, U>: MonoBehaviour 
      where T : SpawningEvent
      where U : System.Enum {
  private int poolMax;
  

  protected abstract string ResolvePath(U type); // Switch through this to Load.Resouce<>("somewhere under Resources")
  protected abstract Stack<T> ResolvePool(U type); // Switch through this to determine the stack with the correct GameObject

  private void AddPool(U type){ // Find a prefab and load it into its class's stack.
    Stack<T> pool = this.ResolvePool(type);
    T prefab = Resources.Load<T>(this.ResolvePath(type));

    pool.Push(prefab);
  }
  
  private void PopulatePool(U type){ // Create new game objects until you have 10.
    Stack<T> pool = this.ResolvePool(type);
    for (int i = 0; i < poolMax; i++) {
      AddPool(type);
    }
  }

  private void CheckMissingFromPool(U type){ // Ensure that there actually is something in the pool. If not, populate.
    Stack<T> pool = this.ResolvePool(type);
    if (pool.Count == 0) {
      this.PopulatePool(type);
    }
  }

  private T GetPrefab(U type){ // Get a GameObject from the pool. Ensure that something's there first.
    CheckMissingFromPool(type);
    Stack<T> pool = this.ResolvePool(type);
    return pool.Pop();
  }

  public T Get(U type, Vector3 position, Quaternion rotation){ // Return a GameObject.
    T fromPool = this.GetPrefab(type);
    T prefab = Instantiate(fromPool, position , fromPool.transform.rotation);
    prefab.AttachSceneObjects(); // Used to attach the GameObject to controllers in the scene: workaround for not being able to attach
    // live Scene Objects from the inspector.
    prefab.gameObject.SetActive(true);
    return prefab;
  }

  public void Release(T spawningEvent){ // Take the object back, or not.
    Stack<T> pool = this.ResolvePool((U)spawningEvent.EventType);
    if (pool.Count < poolMax) {
      pool.Push(spawningEvent);
    }
    
    spawningEvent.gameObject.SetActive(false);
  }

  protected static void CreatePool(Pool<T, U> pool, int poolMax){ // Used in subclass constructors(static factory).
    pool.poolMax = poolMax;
  }
}