using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEventCube instead

public class NegativeEventCube : SpawningEvent {
  
  public class NegativeEventCubeFactory : EventFactoryNegative {
      
    public static NegativeEventCubeFactory CreateNegativeEventCubeFactory(Vector3 position, Quaternion rotation){
      var cubePrefab = Resources.Load<GameObject>("prefabs/nate/NegativeEventCube");
      
      var selfObject = new GameObject("NegativeEventCubeFactory");
      var self =  selfObject.AddComponent<NegativeEventCubeFactory>();
      
      self._prefab = cubePrefab;
      self._position = position;
      self._rotation = rotation;

      return self;
    }

    public override SpawningEvent CreateSpawningEvent(){
      GameObject cubePrefab = Instantiate(_prefab, _position, Quaternion.identity);
      return cubePrefab.GetComponent<SpawningEvent>();
    }
  }
}
