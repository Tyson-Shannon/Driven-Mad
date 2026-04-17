using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEventCube instead

public class NegativeEventCube : SpawningEvent {
  
  public class NegativeEventCubeFactory : EventFactoryNegative {
      
    public static NegativeEventCubeFactory CreateNegativeEventCubeFactory(){
      var cubePrefab = Resources.Load<GameObject>("prefabs/nate/NegativeEventCube");
      
      var selfObject = new GameObject("NegativeEventCubeFactory");
      var self =  selfObject.AddComponent<NegativeEventCubeFactory>();
      
      self._prefab = cubePrefab;

      return self;
    }

    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      GameObject cubePrefab = Instantiate(_prefab, position, rotation);
      return cubePrefab.GetComponent<SpawningEvent>();
    }
  }
}
