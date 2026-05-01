using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryDictionary<T, U> 
    where T : SpawningEvent
    where U : System.Enum {
  private int _factoryCount = 0;
  private Dictionary<int, EventFactory<T, U>> _factories = new Dictionary<int, EventFactory<T, U>>();

  public int GetFactoryCount(){ return _factoryCount; }
  
  protected EventFactory<T, U> SelectEventFactoryInternal(int randomSelection){
    return _factories[randomSelection % _factories.Count];
  }
  public abstract EventFactory<T, U> SelectEventFactory(int randomSelection);

  protected void AddEventFactoryInternal(EventFactory<T, U> factory){
    _factories[_factoryCount] = factory;
    _factoryCount++;
  }
  public abstract void AddEventFactory(EventFactory<T, U> factory);
}

public class PositiveFactoryDictionary<T, U> : FactoryDictionary<T, U> 
  where T : SpawningEvent<T, U>
  where U : System.Enum {
  private static PositiveFactoryDictionary<T, U> _instance;
  
  private PositiveFactoryDictionary(){}
  public static PositiveFactoryDictionary<T, U> CreatePositiveFactoryDictionary(){
    if (_instance == null) {
      _instance = new PositiveFactoryDictionary<T, U>();
    }
    return _instance;
  }

  public override EventFactory<T, U> SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryPositive<T, U>;
  }

  public override void AddEventFactory(EventFactory<T, U> factory){
    AddEventFactoryInternal(factory as EventFactoryPositive<T, U>);
  }
}

public class NegativeFactoryDictionary<T, U> : FactoryDictionary<T, U> 
    where T : SpawningEvent<T, U>
    where U : System.Enum {
  private static NegativeFactoryDictionary<T, U> _instance;

  private NegativeFactoryDictionary(){}
  public static NegativeFactoryDictionary<T, U> CreateNegativeFactoryDictionary(){
    if (_instance == null) {
      _instance = new NegativeFactoryDictionary<T, U>();
    }
    return _instance;
  }

  public override EventFactory<T, U> SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryNegative<T, U>;
  }

  public override void AddEventFactory(EventFactory<T, U> factory){
    AddEventFactoryInternal(factory as EventFactoryNegative<T, U>);
  }
}

public class BossFactoryDictionary<T, U> : FactoryDictionary<T, U> 
    where T : SpawningEvent<T, U>
    where U : System.Enum {
  private static BossFactoryDictionary<T, U> _instance;

  private BossFactoryDictionary(){}
  public static BossFactoryDictionary<T, U> CreateBossFactoryDictionary(){
    if (_instance == null) {
      _instance = new BossFactoryDictionary<T, U>();
    }
    return _instance;
  }

  public override EventFactory<T, U> SelectEventFactory(int randomSelection){
    return SelectEventFactoryInternal(randomSelection) as EventFactoryBoss<T, U>;
  }

  public override void AddEventFactory(EventFactory<T, U> factory){
    AddEventFactoryInternal(factory as EventFactoryBoss<T, U>);
  }
}
