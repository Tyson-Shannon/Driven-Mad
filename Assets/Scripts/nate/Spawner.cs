using System;
using UnityEngine;

public class Spawner : MonoBehaviour {
  private EventFactorySelector _eventSelector;
  private byte _difficultyAdjust = (byte)sbyte.MaxValue;

  #region CONSTRUCTORS
  public unsafe void OnEnable(){
    // Generate a random roll for the factory selector.
    var roll = new PrimitiveUnion();
    roll._valueByte[0] = Byte.MaxValue / 2;
    
    // Create the factories for the factory selector.
    var cubeFactory = NegativeEventCube.NegativeEventCubeFactory.CreateNegativeEventCubeFactory(transform.position, transform.rotation);
    var capsuleFactory = PositiveEventCapsule.PositiveEventCapsuleFactory.CreatePositiveEventCapsuleFactory(transform.position, transform.rotation);
    
    // Make sure that the factory selector is now created.
    _eventSelector = new EventFactorySelector(capsuleFactory, cubeFactory, roll._valueByte[0]);
  } 
  #endregion CONSTRUCTORS

  public void Update(){
    if (_eventSelector != null) {
      var factory = _eventSelector.SelectEventFactory(_difficultyAdjust);
      factory.CreateSpawningEvent();
    }
  }
}
