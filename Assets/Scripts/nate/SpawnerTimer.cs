using UnityEngine;

public class SpawnerTimer : MonoBehaviour {
  private float _bossTime;
  private float _nonBossTime;

  private float _bossTimeElapsed = 0;
  public float BossTime{set{_bossTimeElapsed = value;}}
  private float _nonBossTimeElapsed = 0;
  public float NonBossTime{set{_nonBossTimeElapsed = value;}}

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
    _bossTimeElapsed = 0;
    _nonBossTimeElapsed = 0;
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
