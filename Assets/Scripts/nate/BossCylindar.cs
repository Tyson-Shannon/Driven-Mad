using UnityEngine;

public class BossCylindar : MonoBehaviour
{
    public class BossCylindarFactory : EventFactoryBoss {
      
        public static EventFactoryBoss CreateBossEventCubeFactory(){
            var cubePrefab = Resources.Load<GameObject>("prefabs/nate/NegativeEventCube");
      
            var selfObject = new GameObject("NegativeEventCubeFactory");
            var self =  selfObject.AddComponent<BossCylindarFactory>();
      
            self._prefab = cubePrefab;

            return self;
        }

        public override SpawningEvent CreateSpawningEvent(Vector3 position, Quaternion rotation){
            GameObject cubePrefab = Instantiate(_prefab, position, rotation);
            return cubePrefab.GetComponent<SpawningEvent>();
        }
    }
}
