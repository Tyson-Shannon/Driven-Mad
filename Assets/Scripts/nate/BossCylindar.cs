using UnityEngine;

// Look for PositiveEventCapsule for instructions, this is the same, but uses the NegativeEvent

public class BossCylindar : SpawningEvent {
    public class BossCylindarFactory : EventFactoryBoss {
      
        public BossCylindarFactory(){
            base._prefab = Resources.Load<GameObject>("prefabs/nate/BossCylindar");
            base._registerFactory = true;
        }

        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            GameObject cubePrefab = Instantiate(_prefab, position, rotation);
            return cubePrefab.GetComponent<SpawningEvent>();
        }
    }
}
