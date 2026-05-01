using UnityEngine;

public class PositiveEventCapsule : TestEvent{
  // Do whatever you want here: this is whatever you happen to be spawning.
  
  public class PositiveEventCapsuleFactory : EventFactoryPositive<TestEvent, TestEvent.TestEventType> {
    // If a spawnable event needs any extra state, put it right here.
    // Since it's a nested class, you're not going to be able to put anything in from the inspector.
    
    // The test pools have to be marked as one of each individual immediate subclasses of EventFactory. Unfortunately,
    // that means that they can't all inherit from the same base class. Thus, a singleton exists which holds the pool
    // for the three of them. They should use the same pool.
    TestEvent.TestFactoryPool _pool = TestEvent.TestFactoryPool.CreateAbstractTestFactoryPool();

    public PositiveEventCapsuleFactory(){
      base._registerFactory = false; // We don't want this one to be able to spawn unless we're doing testing with it.
    }

    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      PositiveEventCapsule self = _pool.Pool.Get(TestEvent.TestEventType.CAPSULE, position, rotation) as PositiveEventCapsule;
      self.pool = _pool.Pool;
      return self;
    }
  }
}
