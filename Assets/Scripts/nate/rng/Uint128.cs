using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public unsafe struct UInt128 {
  private fixed ulong _value64[2];

  public UInt128(ulong lower=0, ulong higher=0){
    this._value64[0] = lower;
    this._value64[1] = higher;
  }

  public static UInt128 operator <<(UInt128 operand, int rollLen){
    unchecked {
      rollLen &= 127;

      ulong low = operand._value64[0];
      ulong high = operand._value64[1];

      if (rollLen == 0) return operand;

      if (rollLen < 64) {
        return new UInt128(
          low << rollLen,
          (high << rollLen) | (low >> (64 - rollLen))
        );
      }

      return new UInt128(
        0,
        low << (rollLen - 64)
      );
    }
  }
  
  public static UInt128 operator >>(UInt128 operand, int rollLen){
    unchecked {
      rollLen &= 127;

      ulong low = operand._value64[0];
      ulong high = operand._value64[1];

      if (rollLen == 0) return operand;
      if (rollLen < 64) return new UInt128((low >> rollLen) | (high << (64 - rollLen)), high >> rollLen);
      return new UInt128(high >> (rollLen - 64), 0);
    }
  }
  
  private static void Mul64To128(ulong a, ulong b, out ulong low, out ulong high){
    const ulong mask = 0xFFFFFFFFUL;

    ulong a0 = a & mask;
    ulong a1 = a >> 32;
    ulong b0 = b & mask;
    ulong b1 = b >> 32;

    ulong p00 = a0 * b0;
    ulong p01 = a0 * b1;
    ulong p10 = a1 * b0;
    ulong p11 = a1 * b1;

    ulong middle = (p00 >> 32) + (p10 & mask) + (p01 & mask);

    low = (p00 & mask) | (middle << 32);
    high = p11 + (p10 >> 32) + (p01 >> 32) + (middle >> 32);
  }

  public static UInt128 operator *(UInt128 left, UInt128 right){
    unchecked {
      Mul64To128(left._value64[0], right._value64[0], out ulong low, out ulong high);

      high += left._value64[0] * right._value64[1];
      high += left._value64[1] * right._value64[0];

      return new UInt128(low, high);
    }
  }

  private static ulong AddWithCarry(ulong left, ulong right, out ulong carry){
    ulong result = left + right;
    carry = (result < left) ? 1UL : 0UL;
    return result;
  }
  
  public static UInt128 operator +(UInt128 left, UInt128 right){
    unchecked {
      ulong carry;
      ulong low = AddWithCarry(left._value64[0], right._value64[0],  out carry);

      ulong high = left._value64[1] + right._value64[1] + carry; // finally, we add the high values and potentially
                                                                  // a carry as well.
      return new UInt128(low, high);
    }
  }
  
  public static UInt128 operator +(UInt128 left, ulong right){
    unchecked {
      ulong carry;
      ulong low = AddWithCarry(left._value64[0], right, out carry);

      return new UInt128(low, left._value64[1] + carry);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static explicit operator byte(UInt128 operand){
    return (byte)operand._value64[0];
  }
}
