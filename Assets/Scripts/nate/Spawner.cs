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

  public float SetRegularEventTiming {
    set{_regularEventTiming = value;}
  }
  public float SetBossEventTimingFactor {
    set {
      _bossEventTimingFactor = value;
      _bossEventTiming = _regularEventTiming * _bossEventTiming;
    }
  }

  [SerializeField] private bool _isRunningSerialized;
  private IsRunningCls _isRunningCls = new IsRunningCls();

  #region CONSTRUCTORS
  public unsafe void Start(){
    // Having the _bossEventTiming be a multiple of _regularEventTiming simplifies the timing logic significantly.
    _bossEventTiming = _regularEventTiming * _bossEventTimingFactor;
    
    // Generate a random roll for the factory selector.
    var roll = new PrimitiveUnion();
    roll._valueByte[0] = Byte.MaxValue / 2;
    
    _isRunningCls._isRunning = true;
    // Set up the timer.
    _timer = SpawnerTimer.CreateSpawnerTimer(_bossEventTiming, _regularEventTiming, _isRunningCls);
    
    // Make sure that the factory selector is now created.
    _eventSelector = new EventFactorySelector();
  }
  #endregion CONSTRUCTORS

  public unsafe void Update(){
    if (!_isRunningCls._isRunning) return;
    
    var spawningConditions = new PrimitiveUnion(); // We're using parameters to be extra sure that it starts at 0.
    spawningConditions._valueBool[0] = _timer.SpawnBoss; // 1:
    spawningConditions._valueBool[1] = _timer.SpawnNonBoss; // 2:
    spawningConditions.CompressBools();

    switch ((SpawningFlags)spawningConditions._value16[0]) {
      
      case SpawningFlags.BOSS | SpawningFlags.NON_BOSS:
      case SpawningFlags.BOSS: // Spawn a boss and reset both times: regardless.
        _timer.ResetBoss();
        _eventSelector._isBoss = true;
        Debug.Log($"Spawn flags: {(SpawningFlags)spawningConditions._value16[0]}, isBoss: {_eventSelector._isBoss}");
        goto case SpawningFlags.NON_BOSS;
        
      case SpawningFlags.NON_BOSS: // It isn't time for a boss, then we just do this.
        _timer.ResetNonBoss();
        break;
      
      default:
        goto dontSpawn;
    }
    
    // Regardless of what we spawn, we want to do this
    var factory = _eventSelector.SelectEventFactory(_difficultyAdjust);
    factory.CreateSpawningEvent(transform.position, transform.rotation);
    _eventSelector._isBoss = false; // We never want to leave this on at the end of the update.
    _eventSelector.Reroll();
    
    dontSpawn: ;
  }

  public void StopSpawing(){
    _isRunningCls._isRunning = false;
  }

  public void ResumeSpawing(){
    _isRunningCls._isRunning = true;
  }
}

[Flags]
enum SpawningFlags {
  NONE,
  BOSS,
  NON_BOSS,
}
