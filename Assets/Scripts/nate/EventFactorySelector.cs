public class EventFactorySelector {
  public bool _isBoss = false; // This is to be set from external contexts, and does not belong to the class. Do not write to this.
  
  // We reroll every time we wish to reset.
  private PgcSingleton _prng;
  private PrimitiveUnion _roll;

  // We return a factory by pulling it from its dictionary.
  private PositiveFactoryDictionary _positive;
  private NegativeFactoryDictionary _negative;
  private BossFactoryDictionary _boss;
  
  public EventFactorySelector(){
    // Get the singleton and the roll.
    _prng = PgcSingleton.CreatePgcSingleton();
    _roll = _prng.RandomPrimativeUnion();
    
    // Get the factory dictionaries to pick an event from
    _positive = PositiveFactoryDictionary.CreatePositiveFactoryDictionary();
    _negative = NegativeFactoryDictionary.CreateNegativeFactoryDictionary();
    _boss = BossFactoryDictionary.CreateBossFactoryDictionary();
  }

  public unsafe EventFactory SelectEventFactory(byte difficultyAdjust=0){
    int randomSelection = _roll._sValue32[1];
    FactoryDictionary factoryDict = (_isBoss) ? _boss 
      : (_roll._sValueByte[0] - difficultyAdjust > 0) 
          ? _positive
          : _negative;
      ; 

    // We return this regardless.
    return factoryDict.SelectEventFactory(randomSelection);
  }
}
