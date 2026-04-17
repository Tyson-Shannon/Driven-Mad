using System;
using System.Runtime.InteropServices;

/*
 * This struct needs to contain the methods for the operations in the PRNG, because they will need access to the state's
 * individual components. Otherwise, this struct should be usable as a value that comes out of the RNG, and can be used
 * as any sort of primitive type.
 */
[StructLayout(LayoutKind.Explicit)]
public unsafe struct PrimitiveUnion {
  [FieldOffset(0)]private UInt128 _value128;
  [FieldOffset(0)]public fixed ulong _value64[2];
  [FieldOffset(0)]public fixed long _sValue64[2];
  [FieldOffset(0)]public fixed uint _value32[4];
  [FieldOffset(0)]public fixed int _sValue32[4];
  [FieldOffset(0)]public fixed ushort _value16[8];
  [FieldOffset(0)]public fixed short _sValue16[8];
  [FieldOffset(0)]public fixed char _valueChar[8];
  [FieldOffset(0)]public fixed byte _valueByte[16];
  [FieldOffset(0)]public fixed sbyte _sValueByte[16];
  [FieldOffset(0)]public fixed bool _valueBool[16];

  #region CONSTRUCTORS
  public PrimitiveUnion(ulong low=0, ulong high=0){
    _value64[0] = low;
    _value64[1] = high;
  }
  #endregion //CONSTRUCTORS
  
  #region RNGOps
  internal void LinearCongruence(PrimitiveUnion multiplier, ulong increment){
    _value128 = _value128 * multiplier._value128 + increment;
  }

  private static ulong XorShiftRotate(ulong halfState, byte rotation){
    return (halfState << rotation) | (halfState >> (64 - rotation));
  }

  internal PrimitiveUnion XorShift(){ // Here be there dragons. Exposing the upper half of the state in the result: hope this doesn't affect distribution.
    ulong stateXored = _value64[0] ^ _value64[1];  // Use the upper part of the state to affect the result.
    byte rotation = (byte)(_value128 >> 122);
    
    stateXored = PrimitiveUnion.XorShiftRotate(stateXored, rotation);
    
    return new PrimitiveUnion(
      _value64[1], stateXored
    );
  }
  
  #region DEPRICATED
  // Turns out that this modulo is handled by integer overflow semantics: may as well keep it.
  internal void ModB2(byte power){
    if(power > 128) // We're not going to deal with anything larger than this.
      throw new IndexOutOfRangeException("The power must be greater than 128.");
    
    fixed (PrimitiveUnion* ptr = &this) { // Having the GC suddenly move the pointer during this block would be catastrophic.
      bool powerOverflowed = power >= 64;
      ulong* changedHalf = (powerOverflowed) ? ((ulong*)ptr + 1): (ulong*)ptr;

      if (powerOverflowed) // Need to adjust the power: the lower long should be unaffected arithmetically.
        power -= 64;

      ulong moduloDivisor = (1UL << power) - 1;
      
      *changedHalf &= moduloDivisor;
    }
  }
  #endregion DEPRICATED
  #endregion RNGOps

  public void CompressBools(){
    int iterationLimit = sizeof(PrimitiveUnion) / sizeof(bool);
    
    for (int i = 0; i < iterationLimit; i++) {
      _valueByte[i] <<= i;
    }

    for (int i = 0; i < iterationLimit; i++) {
      _value16[0] += _valueByte[i];
    }
  }
}

public class IndexOutOfRangeException : Exception{
  public IndexOutOfRangeException(string msg) : base(msg) { }
}