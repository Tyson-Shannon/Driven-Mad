using UnityEngine;

// The basic ad hōc factory type that instantiates a prefab at a location.
public abstract class EventFactory {
  protected GameObject _prefab;
  protected bool _registerFactory = false;
  
  public bool ShouldRegister => _registerFactory;

  public abstract SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation);
}

#region CHILD_CLASSES

// The following child classes serve as markers for random selection logic.
public abstract class EventFactoryPositive : EventFactory {}
public abstract class EventFactoryNegative : EventFactory {}
public abstract class EventFactoryBoss : EventFactory {}
#endregion CHILD_CLASSES