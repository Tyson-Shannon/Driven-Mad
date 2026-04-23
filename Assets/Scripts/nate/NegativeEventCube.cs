using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEvent

public class NegativeEventCube : SpawningEvent {
  public class NegativeEventCubeFactory : EventFactoryNegative {
      
    public NegativeEventCubeFactory(){
      base._prefab = Resources.Load<GameObject>("prefabs/nate/NegativeEventCube");
      base._registerFactory = true;
    }

    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      GameObject cubePrefab = Instantiate(_prefab, position, rotation);
      return cubePrefab.GetComponent<SpawningEvent>();
    }
  }
}
