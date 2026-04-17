using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryDictionary {
  private int _factoryCount = 0;
  private Dictionary<int, EventFactory> _factories = new Dictionary<int, EventFactory>();

  public int GetFactoryCount(){ return _factoryCount; }
  
  protected EventFactory SelectEventFactoryInternal(int randomSelection){
    return _factories[randomSelection];
  }
  public abstract EventFactory SelectEventFactory(int randomSelection);

  protected void AddEventFactoryInternal(EventFactory factory){
    _factories[_factoryCount] = factory;
    _factoryCount++;
  }
  public abstract void AddEventFactory(EventFactory factory);
}

public class PositiveFactoryDictionary : FactoryDictionary {
  private static PositiveFactoryDictionary _instance;
  public FactoryDictionary CreatePositiveFactoryDictionary(){
    if (_instance == null) {
      _instance = new PositiveFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryPositive;
  }

  public override void AddEventFactory(EventFactory factory){
    AddEventFactoryInternal(factory as EventFactoryPositive);
  }
}

public class NegativeFactoryDictionary : FactoryDictionary {
  private static NegativeFactoryDictionary _instance;

  public FactoryDictionary CreateNegativeFactoryDictionary(){
    if (_instance == null) {
      _instance = new NegativeFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryNegative;
  }

  public override void AddEventFactory(EventFactory factory){
    AddEventFactoryInternal(factory as EventFactoryNegative);
  }
}

public class BossFactoryDictionary : FactoryDictionary {
  private static BossFactoryDictionary _instance;

  public BossFactoryDictionary CreateBossFactoryDictionary(){
    if (_instance == null) {
      _instance = new BossFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryBoss;
  }

  public override void AddEventFactory(EventFactory factory){
    AddEventFactoryInternal(factory as EventFactoryBoss);
  }
}
