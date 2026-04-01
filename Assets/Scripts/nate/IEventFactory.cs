using UnityEngine;

public abstract class EventFactory : MonoBehaviour {
  protected GameObject _prefab;
  public Vector3 _position;
  public Quaternion _rotation;
  
  public abstract SpawningEvent CreateSpawningEvent();
}

public abstract class EventFactoryPositive : EventFactory {}
public abstract class EventFactoryNegative  : EventFactory {}
