using System;
using System.Collections.Generic;

public abstract class FactoryDictionary{
  private Dictionary<uint, EventFactory> _factories = new Dictionary<uint, EventFactory>();

  public uint GetFactoryCount(){ return (uint)_factories.Count; }
  
  protected EventFactory SelectEventFactoryInternal(uint randomSelection){
    return _factories[randomSelection % (uint)_factories.Count];
  }
  public abstract EventFactory SelectEventFactory(uint randomSelection);

  protected void AddEventFactoryInternal(EventFactory factory){
    _factories[(uint)_factories.Count] = factory;
  }
  public abstract void AddEventFactory(EventFactory factory);
  
  protected static bool IsFactoryCorrect(Type type, Type openFactory){
    while (type != null && type != openFactory) {
      if (
        type.IsGenericType
        && type.GetGenericTypeDefinition() == openFactory
      ) return true;
      
      type = type.BaseType;
    }
    
    return false;
  }
}

public class PositiveFactoryDictionary : FactoryDictionary {
  private static PositiveFactoryDictionary _instance;
  
  private PositiveFactoryDictionary(){}
  public static PositiveFactoryDictionary CreatePositiveFactoryDictionary(){
    if (_instance == null) {
      _instance = new PositiveFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(uint randomSelection){
    var factory = base.SelectEventFactoryInternal(randomSelection);
    if(!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryPositive<,>)))
      return null;
    return factory;
  }

  public override void AddEventFactory(EventFactory factory){
    if (!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryPositive<,>))) return;
    base.AddEventFactoryInternal(factory);
  }

}

public class NegativeFactoryDictionary : FactoryDictionary {
  private static NegativeFactoryDictionary _instance;

  private NegativeFactoryDictionary(){}
  public static NegativeFactoryDictionary CreateNegativeFactoryDictionary(){
    if (_instance == null) {
      _instance = new NegativeFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(uint randomSelection){
    var factory = base.SelectEventFactoryInternal(randomSelection);
    if (!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryNegative<,>)))
      return null;
    return factory;
  }

  public override void AddEventFactory(EventFactory factory){
    if (!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryNegative<,>))) return;
    base.AddEventFactoryInternal(factory);
  }
}

public class BossFactoryDictionary : FactoryDictionary {
  private static BossFactoryDictionary _instance;

  private BossFactoryDictionary(){}
  public static BossFactoryDictionary CreateBossFactoryDictionary(){
    if (_instance == null) {
      _instance = new BossFactoryDictionary();
    }
    return _instance;
  }

  public override EventFactory SelectEventFactory(uint randomSelection){
    var factory = base.SelectEventFactoryInternal(randomSelection);
    if (!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryBoss<,>)))
      return null;
    return factory;
  }

  public override void AddEventFactory(EventFactory factory){
    if (!FactoryDictionary.IsFactoryCorrect(factory.GetType(), typeof(EventFactoryBoss<,>))) return;
    AddEventFactoryInternal(factory);
  }
}
