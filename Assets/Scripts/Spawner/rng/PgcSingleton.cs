// PGC-XSL-RR-RR random number generator.

using System;

public class PgcSingleton {
  // Singleton instance
  #nullable enable
  private static PgcSingleton self;
  #nullable disable

  private PrimitiveUnion _state; // The background state used by PGC-XSL-RR-RR.
  private readonly ulong _increment = 0xdeadbeafdeadbeaf;
  private readonly PrimitiveUnion _multiplier =  new PrimitiveUnion(0x4385DF649FCCF645UL,0x2360ED051FC65DA4UL);

  #region SINGLETONCONSTRUCTOR
  // SplitMix64-style mixer (Sebastiano Vigna)
  private static ulong MixSeed(ulong seed){
    unchecked {
      seed ^= seed >> 30;
      seed *= 0xBF58476D1CE4E5B9UL;
      seed ^= seed >> 27;
      seed *= 0x94D049BB133111EBUL;
      seed ^= seed >> 31;
      return seed;
    }
  }

  private void SeedInternal(ulong seed){
    if (seed == 0) {
      seed = (ulong)DateTime.Now.Ticks; // seed=0 sacrifices too much randomness.
    }

    ulong state = PgcSingleton.MixSeed(seed);
    
    _state = new PrimitiveUnion(state, PgcSingleton.MixSeed(state));
  }
    
  private PgcSingleton(ulong seed){
    this.SeedInternal(seed);
  }

  public static PgcSingleton CreatePgcSingleton(ulong seed=0){
    if (PgcSingleton.self == null) {
      PgcSingleton.self = new PgcSingleton(seed);
    }
    else {
      PgcSingleton.self.SeedInternal(seed);
    }

    return PgcSingleton.self;
  }
  #nullable disable
  #endregion SINGLETONCONSTRUCTOR

  public void Seed(ulong seed){
    SeedInternal(seed);
  }
  
  private PrimitiveUnion Random(){  // The steps themselves are handled by the UInt128Random: think of it as a domain-specific datatype.
    _state = _state.LinearCongruence(_multiplier, _increment);
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