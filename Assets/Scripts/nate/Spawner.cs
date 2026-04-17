using System;
using UnityEngine;

public class Spawner : MonoBehaviour {
  private EventFactorySelector _eventSelector;
  private byte _difficultyAdjust = 0; // We'll figure out what to do with this later.
  
  private SpawnerTimer _timer;

  // We decide how long the wait should be from the inspector. Then, the timer has to respect that.
  [SerializeField] private float _regularEventTiming;
  [SerializeField] private float _bossEventTimingFactor;
  private float _bossEventTiming;

  [SerializeField] private bool _isRunning;

  #region CONSTRUCTORS
  public unsafe void Start(){
    // Having the _bossEventTiming be a multiple of _regularEventTiming simplifies the timing logic significantly.
    _bossEventTiming = _regularEventTiming * _bossEventTimingFactor;
    
    // Generate a random roll for the factory selector.
    var roll = new PrimitiveUnion();
    roll._valueByte[0] = Byte.MaxValue / 2;
    
    // Set up the timer.
    _timer = SpawnerTimer.CreateSpawnerTimer(_bossEventTiming, _regularEventTiming);
    _timer.IsRunning = _isRunning; // It would be so nice to make the field in the timer a ref: exclusive to Unity 10.
    
    // Make sure that the factory selector is now created.
    _eventSelector = new EventFactorySelector();
  } 
  #endregion CONSTRUCTORS

  public unsafe void Update(){
    _timer.IsRunning = _isRunning;
    if (!_isRunning) return;

    var spawningConditions = new PrimitiveUnion(0, 0); // We're using parameters to be extra sure that it starts at 0.
    spawningConditions._valueBool[0] = _timer.SpawnBoss; // 1:
    spawningConditions._valueBool[1] = _timer.SpawnNonBoss; // 2:
    spawningConditions.CompressBools();

    switch ((SpawningFlags)spawningConditions._value16[0]) {
      
      case SpawningFlags.BOSS | SpawningFlags.NON_BOSS:
      case SpawningFlags.BOSS: // Spawn a boss and reset both times: regardless.
        _timer.BossTime = 0;
        _eventSelector._isBoss = true;
        goto case SpawningFlags.NON_BOSS;
        
      case SpawningFlags.NON_BOSS: // It it isn't time for a boss, then we just do this.
        _timer.NonBossTime = 0;
        
        // Regardless of what we spawn, we want to do this
        var factory = _eventSelector.SelectEventFactory(_difficultyAdjust);
        factory.CreateSpawningEvent(transform.position, transform.rotation);
        _eventSelector._isBoss = false;
        break;
      
      case SpawningFlags.NONE:
        break;
    }

  }

  public void StopSpawing(){
    _isRunning = false;
    _timer.IsRunning = false;
  }
}

[Flags]
enum SpawningFlags {
  NONE,
  BOSS,
  NON_BOSS,
}
