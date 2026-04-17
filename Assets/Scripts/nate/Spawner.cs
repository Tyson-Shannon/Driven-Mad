using System;
using System.Timers;
using UnityEngine;

public class Spawner : MonoBehaviour {
  private EventFactorySelector _eventSelector;
  private byte _difficultyAdjust = (byte)sbyte.MaxValue;
  SpawnerTimer _timer;

  [SerializeField] private int _regularEventTiming;
  [SerializeField] private int _bossEventTimingFactor;
  private int _bossEventTiming;

  #region CONSTRUCTORS
  public unsafe void Awake(){
    // Having the _bossEventTiming be a multiple of _regularEventTiming simplifies the timing logic significantly.
    _bossEventTiming = _regularEventTiming * _bossEventTimingFactor;
    
    // Generate a random roll for the factory selector.
    var roll = new PrimitiveUnion();
    roll._valueByte[0] = Byte.MaxValue / 2;
    
    // Make sure that the factory selector is now created.
    _eventSelector = new EventFactorySelector(roll._sValueByte[0]);
  } 
  #endregion CONSTRUCTORS

  public void Update(){
    var factory = _eventSelector.SelectEventFactory(_difficultyAdjust);
    factory.CreateSpawningEvent(transform.position, transform.rotation);
  }
}
