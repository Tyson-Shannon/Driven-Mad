using UnityEngine;

public class SpawnerTimer : MonoBehaviour {
  private float _bossTime;
  private float _nonBossTime;

  private float _bossTimeElapsed = 0;
  private float _nonBossTimeElapsed = 0;

  private IsRunningCls _isRunning;
  
  private bool _spawnBoss = false;
  public bool SpawnBoss => _spawnBoss;
  private bool _spawnNonBoss = false;
  public bool SpawnNonBoss => _spawnNonBoss;

  public static SpawnerTimer CreateSpawnerTimer(float bossTime, float nonBossTime, IsRunningCls isRunningCls){
    var spawnerTimerObj = new GameObject("SpawnerTimer");
    var self =  spawnerTimerObj.AddComponent<SpawnerTimer>();

    self._bossTime = bossTime;
    self._nonBossTime = nonBossTime;
    self._isRunning = isRunningCls;

    return self;
  }

  private void ResetInternal(){
    _bossTimeElapsed = 0;
    _nonBossTimeElapsed = 0;
  }

  private void Reset(){
    _bossTimeElapsed = 0;
    _nonBossTimeElapsed = 0;
    _spawnBoss = false;
    _spawnNonBoss = false;
  }
  
  public void ResetBoss(){
    Reset();
  }

  public void ResetNonBoss(){
    _nonBossTimeElapsed = 0;
    _spawnNonBoss = false;
  }
  
  public void Update(){
    // Make sure we don't run if we're not supposed to be running.
    if (!_isRunning._isRunning) {
      Reset();
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

// Workaround for the lack of ref.
public class IsRunningCls {
  public bool _isRunning;
}
