using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EventFactorySelector {
  private EventFactoryPositive _positiveEvent = null;
  private EventFactoryNegative _negativeEvent = null;
  private byte _roll;

  public EventFactorySelector(EventFactoryPositive positiveEvent, EventFactoryNegative negativeEvent, byte roll){
    _positiveEvent = positiveEvent;
    _negativeEvent = negativeEvent;
    _roll = roll;
  }

  [CanBeNull]
  public unsafe EventFactory SelectEventFactory(byte difficultyAdjust){
    var factoriesPresent = new PrimitiveUnion();
    factoriesPresent._valueBool[0] = _positiveEvent == null;
    factoriesPresent._valueBool[1] = _negativeEvent == null;
    factoriesPresent.CompressBools();

    switch (factoriesPresent._value16[0]) {
      case 2:
        return _positiveEvent;
      case 1:
        return _negativeEvent;
      case 3:
        return null;
    }

    return (
      (_roll <= difficultyAdjust) ? _positiveEvent : _negativeEvent
    );
  } 
}
