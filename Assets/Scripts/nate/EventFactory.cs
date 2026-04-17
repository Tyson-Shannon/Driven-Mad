using UnityEngine;

// The basic ad hōc factory type that instantiates a prefab at a location.
public abstract class EventFactory : MonoBehaviour {
  protected GameObject _prefab;
  protected Vector3 _spawnPosition;

  public abstract SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation);
  protected virtual bool IgnoreInstantiation(){return true;}
}

#region CHILD_CLASSES

// The following child classes serve as markers for random selection logic.
public abstract class EventFactoryPositive : EventFactory {
  protected override bool IgnoreInstantiation(){return true;}
}

public abstract class EventFactoryNegative : EventFactory {
  protected override bool IgnoreInstantiation(){return true;}
}

public abstract class EventFactoryBoss : EventFactory {
  protected override bool IgnoreInstantiation(){return true;}
}
#endregion CHILD_CLASSES