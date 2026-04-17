// PGC-XSL-RR-RR random number generator.

using System;

public class PgcSingleton {
  // Singleton instance
  #nullable enable
  private static PgcSingleton self;
  #nullable disable

  private PrimitiveUnion _state; // The background state used by PGC-XSL-RR-RR.
  private readonly ulong _increment = 0xdeadbeafdeadbeaf;
  private readonly PrimitiveUnion _multiplier =  new PrimitiveUnion(0x2360ED051FC65DA4UL,0x4385DF649FCCF645UL);

  #region SINGLETONCONSTRUCTOR
  private PgcSingleton(ulong seed){
    if (seed <= byte.MaxValue) {
      seed = (ulong)DateTime.Now.Ticks; // seed=0 sacrifices too much randomness.
    }
    
    _state = new PrimitiveUnion(seed, seed * seed);
  }

  public static PgcSingleton CreatePgcSingleton(ulong seed=0){
    if (PgcSingleton.self == null) {
      PgcSingleton.self = new PgcSingleton(seed);
    }

    return PgcSingleton.self;
  }
  #nullable disable
  #endregion SINGLETONCONSTRUCTOR

  
  private PrimitiveUnion Random(){  // The steps themselves are handled by the UInt128Random: think of it as a domain-specific datatype.
    _state.LinearCongruence(_multiplier, _increment);
    return _state.XorShift();
  }

  public PrimitiveUnion RandomPrimativeUnion(){
    return this.Random();
  }

  public unsafe PrimitiveUnion RandomPrimitiveUnionB2(){
    PrimitiveUnion low = this.Random();
    PrimitiveUnion high = this.Random();
    return new PrimitiveUnion(low._value64[0], high._value64[0]);
  }
}