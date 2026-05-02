using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEvent

public class BossCylinder : TestEvent {
    public override void AttachSceneObjects(){
        return;
    }

    void Update(){
        transform.Translate(new Vector3(0, 0, -(.4f * Time.deltaTime * 10)));
    }
    
    public class BossCylinderFactory : EventFactoryBoss<TestEvent, TestEvent.TestEventType> {
        TestFactoryPool _pool = TestFactoryPool.CreateAbstractTestFactoryPool();
        
        // Until we have actual boss prefabs, we'll allow this to spawn.
        
        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            BossCylinder self = (BossCylinder)_pool.Pool.Get(TestEvent.TestEventType.CYLINDER, position, rotation);
            self.pool = _pool.Pool;
            return self;
        }
    }
}
