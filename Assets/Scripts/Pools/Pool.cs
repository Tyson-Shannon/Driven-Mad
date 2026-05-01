
using System.Collections.Generic;
using UnityEngine;


public abstract class Pool<T, U>: MonoBehaviour 
      where T : SpawningEvent
      where U : System.Enum {
  private int poolMax;
  

  protected abstract string ResolvePath(U type);
  protected abstract Stack<T> ResolvePool(U type);

  private void AddPool(U type){
    Stack<T> pool = this.ResolvePool(type);
    T prefab = Resources.Load<T>(this.ResolvePath(type));
      
    pool.Push(prefab);
  }
  private void PopulatePool(U type){
    Stack<T> pool = this.ResolvePool(type);
    for (int i = 0; i < poolMax; i++) {
      AddPool(type);
    }
  }

  private void CheckMissingFromPool(U type){
    Stack<T> pool = this.ResolvePool(type);
    if (pool.Count == 0) {
      this.PopulatePool(type);
    }
  }

  private T GetPrefab(U type){
    CheckMissingFromPool(type);
    Stack<T> pool = this.ResolvePool(type);
    return pool.Pop();
  }

  public T Get(U type, Vector3 position, Quaternion rotation){
    T prefab = this.GetPrefab(type);
    return Instantiate(prefab, position, rotation);
  }

  public void Release(T monoBehavior){
    Stack<T> pool = this.ResolvePool(monoBehavior.type);
    if (pool.Count < poolMax) {
      pool.Push(monoBehavior);
    }
    
    monoBehavior.gameObject.SetActive(false);
  }

  protected static void CreatePool(Pool<T, U> pool, int poolMax){
    pool.poolMax = poolMax;
  }
}