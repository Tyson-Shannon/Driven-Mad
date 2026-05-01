using UnityEngine;

#region NON_GENERIC_TYPE

public abstract class EventFactory {
  protected bool _registerFactory = true; // Will be skipped for all classes that are abstract.
  
  public abstract SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation);
}
#endregion NON_GENERIC_TYPE

// The basic ad hoc factory type that instantiates a prefab at a location.
public abstract class EventFactory<T, U> : EventFactory
    where T : SpawningEvent
    where U : System.Enum {
  protected Pool<T, U> pool;
}

#region CHILD_CLASSES

// The following child classes serve as markers for random selection logic.
public abstract class EventFactoryPositive<T, U> : EventFactory<T, U> 
  where T : SpawningEvent
  where U : System.Enum {
}

public abstract class EventFactoryNegative<T, U> : EventFactory<T, U> 
  where T : SpawningEvent
  where U : System.Enum {
}

public abstract class EventFactoryBoss<T, U> : EventFactory<T, U> 
  where T : SpawningEven
  where U : System.Enum {
}
#endregion CHILD_CLASSES