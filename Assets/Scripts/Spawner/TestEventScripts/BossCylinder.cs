using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEvent

public class BossCylinder : TestEvent {
    public class BossCylinderFactory : EventFactoryBoss<TestEvent, TestEvent.TestEventType> {
        TestFactoryPool _pool = TestFactoryPool.CreateAbstractTestFactoryPool();
        
        // Until we have actual boss prefabs, we'll allow this to spawn.
        
        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            BossCylinder self = _pool.Pool.Get(TestEvent.TestEventType.CYLINDER, position, rotation) as BossCylinder;
            self.pool = _pool.Pool;
            return self;
        }
    }
}
