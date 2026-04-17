using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EventFactorySelector {
  public bool _isBoss = false; // This is to be set from external contexts, and does not belong to the class. Do not write to this.
  private sbyte _roll;
  private EventFactory _factory;
  
  #region QUICK_SELECT

  public EventFactoryBoss Boss { get; set; }
  public EventFactoryPositive Positive { get; set; }
  public EventFactoryNegative Negative { get; set; }
  
  #endregion QUICK_SELECT

  public EventFactorySelector(sbyte roll){
    _roll = roll;
  }

  [CanBeNull]
  public EventFactory SelectEventFactory(byte difficultyAdjust){
    if (_isBoss) {
      _factory = this.SelectBossFactory();
    }
    else {
      _factory = (_roll - difficultyAdjust < 0) ? this.SelectPositiveEventFactory() : this.SelectNegativeEventFactory();
    }

    // We return this regardless.
    return _factory;
  }

  private EventFactory SelectBossFactory(){
    return this.Boss;
  }
  
  private EventFactory SelectPositiveEventFactory(){
    return this.Positive;
  }

  private EventFactory SelectNegativeEventFactory(){
    return this.Negative;
  }
}
