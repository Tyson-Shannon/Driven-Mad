using UnityEngine;

public class PositiveEventCapsule : SpawningEvent {
  // Here, you can put in a static factory method that sets up further class components after Inatantiate is called.
  // As well as anything else.
  
  public class PositiveEventCapsuleFactory : EventFactoryPositive {
    // Every gameobject with a mesh renderer is going to need a prefab, position, and rotation. The abstract base class
    // contains the state for those as public fields, and can be modified within the factory ad nauseam.
    
    // This is the factory's constructor. It must be static, as it will create the instance.
    public static PositiveEventCapsuleFactory CreatePositiveEventCapsuleFactory(Vector3 position, Quaternion rotation){
      // First, create the prefab, and put it in a folder called "Resources", which will be spelled exactly as such and
      // directly in Assets. When you call Resources.Load(), the Unity engine will look in that folder from a relative 
      // path.
      var prefab = Resources.Load<GameObject>("prefabs/nate/PositiveEventCapsule");
      
      // Then, create the factory itself as you would with any other monobehavior.
      var selfObject = new GameObject("PositiveEventCapsuleFactory");
      var self =  selfObject.AddComponent<PositiveEventCapsuleFactory>(); // "this" is a reserved keyword, so we use self instead.
      
      // Next, we assign all the fields: prefab, position, rotation.
      self._prefab = prefab;
      self._position = position;
      self._rotation = rotation;
      
      // Finally, we return the instance.
      return self;
    }

    public override SpawningEvent CreateSpawningEvent(){
      GameObject capsuleObject = Instantiate(_prefab, _position, Quaternion.identity);
      return capsuleObject.GetComponent<SpawningEvent>(); // We'll need to figure out a way to avoid doing this in a hot loop.
    }
  }
}
