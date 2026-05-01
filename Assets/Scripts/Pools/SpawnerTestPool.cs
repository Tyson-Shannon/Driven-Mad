using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTestPool : Pool<TestEvent, TestEvent.TestEventType> {
  private Stack<TestEvent> positivePool = new Stack<TestEvent>();
  private Stack<TestEvent> negativePool = new Stack<TestEvent>();
  private Stack<TestEvent> bossPool = new Stack<TestEvent>();
  
  protected override string ResolvePath(TestEvent.TestEventType type){
    switch (type) {
      case TestEvent.TestEventType.CAPSULE: return "Prefab/TestObject/PositiveEventCapsule";
      case TestEvent.TestEventType.CUBE: return "Prefab/TestObject/NegativeEventCube";
      case TestEvent.TestEventType.CYLINDER: return "Prefab/TestObject/BossCylinder";
    }
    return null;
  }

  protected override Stack<TestEvent> ResolvePool(TestEvent.TestEventType type){
    switch (type) {
      case TestEvent.TestEventType.CAPSULE: return positivePool;
      case TestEvent.TestEventType.CUBE: return negativePool;
      case TestEvent.TestEventType.CYLINDER: return bossPool;
    }
    return null;
  }

  public static SpawnerTestPool CreateTestPool(int poolMax){
    var selfObj = new GameObject("SpawnerTestPool");
    var self = selfObj.AddComponent<SpawnerTestPool>();
    
    Pool<TestEvent, TestEvent.TestEventType>.CreatePool(self, poolMax);
    
    return self;
  }
}
