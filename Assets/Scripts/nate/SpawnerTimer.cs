using UnityEngine;

public class SpawnerTimer : MonoBehaviour {
  private float _bossTime;
  public float BossTime{set{_bossTime = value;}}
  private float _nonBossTime;
  public float NonBossTime{set{_nonBossTime = value;}}

  private float _bossTimeElapsed = 0;
  private float _nonBossTimeElapsed = 0;

  private bool _isRunning = false;
  public bool IsRunning {
    get{return _isRunning;}
    set{_isRunning = value;}
  }
  
  private bool _spawnBoss = false;
  public bool SpawnBoss{get{return _spawnBoss;}}
  private bool _spawnNonBoss = false;
  public bool SpawnNonBoss{get{return _spawnNonBoss;}}

  public static SpawnerTimer CreateSpawnerTimer(float bossTime, float nonBossTime){
    var spawnerTimerObj = new GameObject("SpawnerTimer");
    var self =  spawnerTimerObj.AddComponent<SpawnerTimer>();

    self._bossTime = bossTime;
    self._nonBossTime = nonBossTime;
    self._isRunning = false;

    return self;
  }

  private void ResetInternal(){
    _bossTime = 0;
    _nonBossTime = 0;
  }

  public void Reset(){
    ResetInternal();
    _spawnBoss = false;
    _spawnNonBoss = false;
  }
  
  public unsafe void Update(){
    // Make sure we don't run if we're not supposed to be running.
    if (!IsRunning) {
      ResetInternal();
      return;
    }
    
    // Increase the time depending on the frames.
    _bossTimeElapsed += Time.deltaTime;
    _nonBossTimeElapsed += Time.deltaTime;
    
    // Allow external contexts to determine if a boss is supposed to spawn or not.
    if (_bossTimeElapsed > _bossTime) _spawnBoss = true;
    if (_nonBossTimeElapsed > _nonBossTime) _spawnNonBoss = true;
  }
}
