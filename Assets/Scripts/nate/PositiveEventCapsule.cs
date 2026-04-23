using UnityEngine;

public class PositiveEventCapsule : SpawningEvent {
  // Do whatever you want here: this is whatever you happen to be spawning.
  
  public class PositiveEventCapsuleFactory : EventFactoryPositive {
    // If a spawnable event needs any extra state, put it right here.
    // Since it's a nested class, you're not going to be able to put anything in from the inspector.

    public PositiveEventCapsuleFactory(){
      // Load the prefab from the "Resources" directory. The path is local to wherever under the "Resources" tree it is.
      base._prefab = Resources.Load<GameObject>("prefabs/nate/PositiveEventCapsule");
      base._registerFactory = true;
    }

    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      GameObject capsuleObject = Instantiate(_prefab, position, rotation);
      return capsuleObject.GetComponent<SpawningEvent>(); // We'll need to figure out a way to avoid doing this in a hot loop.
      // Or, grab the object from the pool.
    }
  }
}
