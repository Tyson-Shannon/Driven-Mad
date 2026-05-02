using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEvent

public class NegativeEventCube: TestEvent {
  public override void AttachSceneObjects(){
    return;
  }
  public class NegativeEventCubeFactory : EventFactoryNegative<TestEvent, TestEvent.TestEventType> {
    TestFactoryPool _pool = TestFactoryPool.CreateAbstractTestFactoryPool();

    public NegativeEventCubeFactory(){
      base._registerFactory = false;
    }
    
    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      NegativeEventCube self = (NegativeEventCube)_pool.Pool.Get(TestEvent.TestEventType.CUBE, position, rotation);
      self.pool = _pool.Pool;
      return self;
    }
  }
}
