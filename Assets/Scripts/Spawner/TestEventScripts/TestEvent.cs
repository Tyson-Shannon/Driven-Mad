using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestEvent : SpawningEvent<TestEvent, TestEvent.TestEventType>
{
  public enum TestEventType {
    CAPSULE,
    CUBE,
    CYLINDER,
  }

  protected static void CreateAbstractTestEvent(
    SpawningEvent<TestEvent, TestEvent.TestEventType> self, 
    TestEventType type
  ){
    SpawningEvent<TestEvent, TestEvent.TestEventType>.CreateAbstractSpawningEvent(self, type);
  }

  public class TestFactoryPool : EventFactory<TestEvent, TestEvent.TestEventType> {
    public Pool<TestEvent, TestEvent.TestEventType> Pool { get { return base.pool; } }
    private static TestFactoryPool _instance;

    private TestFactoryPool(){
      base._registerFactory = false;
      base.pool = SpawnerTestPool.CreateTestPool(10);
    }

    public static TestFactoryPool CreateAbstractTestFactoryPool(){
      if (_instance == null) {
        _instance = new TestFactoryPool();
      }
      
      return _instance;
    }
    
    public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
      Debug.Log("TestFactoryPool isn't supposed to get instantiated");
      return null;
    }
  }
}
