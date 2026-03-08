using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public unsafe struct UInt128 {
  private fixed ulong _value64[2];

  public UInt128(ulong lower=0, ulong higher=0){
    this._value64[0] = lower;
    this._value64[1] = higher;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static ulong ShiftSelection(ulong value, int rollLen, bool isLeft){
    if (isLeft) {
      return value << rollLen;
    }
    
    return value >> rollLen;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static UInt128 ShiftUint(UInt128 operand, int rollLen, bool isLeft){
    rollLen &= int.MaxValue;
    UInt128 result = operand;
    ulong lowerOverflow = ShiftSelection(result._value64[0], rollLen, !isLeft);
    
    result._value64[0] = ShiftSelection(result._value64[0], rollLen, isLeft);
    result._value64[1] = ShiftSelection(result._value64[1], rollLen, isLeft);
    result._value64[1] |= lowerOverflow;
    
    return result;
  }
  
  public static UInt128 operator <<(UInt128 operand, int rollLen){
    return ShiftUint(operand, rollLen, true);
  }
  
  public static UInt128 operator >>(UInt128 operand, int rollLen){
    return ShiftUint(operand, rollLen, false);
  }

  public static UInt128 operator *(UInt128 left, UInt128 right){
    var result = new UInt128();
    
    *(decimal*)&result = *(decimal*)&left._value64 * *(decimal*)&right._value64;
    
    return result;
  }

  public static UInt128 operator +(UInt128 left, UInt128 right){
    var result = new UInt128();
    
    *(decimal*)&result = *(decimal*)&left._value64 + *(decimal*)&right._value64;
    
    return result;
  }
  
  public static UInt128 operator +(UInt128 left, ulong right){
    var result = new UInt128();
    
    *(decimal*)&result = *(decimal*)&left._value64 + right;
    
    return result;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static explicit operator byte(UInt128 operand){
    return (byte)operand._value64[0];
  }
}
