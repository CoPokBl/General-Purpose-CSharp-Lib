using System.Globalization;
using System.Numerics;

namespace GeneralPurposeLib; 

public class Number {
    private double _value;
    public double DoubleValue {
        get => _value;
        set => _value = value;
    }
    public float FloatValue {
        get => (float)_value;
        set => _value = value;
    }
    public int IntValue {
        get => (int)_value;
        set => _value = value;
    }
    public static Number Instance = new();

    public Number(double val) { 
        _value = val;
    }

    public Number() { }

    public static implicit operator Number(int value) {
        return new Number(value);
    }
    
    public static implicit operator Number(double value) {
        return new Number(value);
    }
    
    public static implicit operator Number(float value) {
        return new Number(value);
    }
    
    public static implicit operator Number(byte value) {
        return new Number(value);
    }
    
    public static implicit operator Number(short value) {
        return new Number(value);
    }
    
    public static implicit operator Number(long value) {
        return new Number(value);
    }
    
    public static implicit operator Number(ulong value) {
        return new Number(value);
    }

    public static implicit operator float(Number value) {
        return value.FloatValue;
    }
    
    public static implicit operator double(Number value) {
        return value.DoubleValue;
    }
    
    public static implicit operator int(Number value) {
        return value.IntValue;
    }
    
    public static implicit operator byte(Number value) {
        return (byte)value.DoubleValue;
    }
    
    public static implicit operator short(Number value) {
        return (short)value.DoubleValue;
    }
    
    public static implicit operator long(Number value) {
        return (long)value.DoubleValue;
    }
    
    public static implicit operator ulong(Number value) {
        return (ulong)value.DoubleValue;
    }

    public static Number operator +(Number a, Number b) {
        return a._value + b._value;
    }
    
    public static Number operator -(Number a, Number b) {
        return a._value - b._value;
    }
    
    public static Number operator *(Number a, Number b) {
        return a._value * b._value;
    }
    
    public static Number operator /(Number a, Number b) {
        return a._value / b._value;
    }
    
    public static Number operator %(Number a, Number b) {
        return a._value % b._value;
    }
    
    public static Number operator ++(Number a) {
        return a._value++;
    }
    
    public static Number operator --(Number a) {
        return a._value--;
    }
    
    public static bool operator ==(Number a, Number b) {
        return a._value == b._value;
    }
    
    public static bool operator !=(Number a, Number b) {
        return a._value != b._value;
    }
    
    public static bool operator <(Number a, Number b) {
        return a._value < b._value;
    }
    
    public static bool operator >(Number a, Number b) {
        return a._value > b._value;
    }
    
    public static bool operator <=(Number a, Number b) {
        return a._value <= b._value;
    }
    
    public static bool operator >=(Number a, Number b) {
        return a._value >= b._value;
    }
    
    public override bool Equals(object obj) {
        return obj is Number number && _value == number._value;
    }
    
    public override int GetHashCode() {
        return _value.GetHashCode();
    }
    
    public override string ToString() {
        return _value.ToString();
    }
    
    public static Number Parse(string s) {
        return double.Parse(s);
    }
    
    public static Number Parse(string s, NumberStyles style) {
        return double.Parse(s, style);
    }
    
    public static Number Parse(string s, NumberStyles style, IFormatProvider provider) {
        return double.Parse(s, style, provider);
    }
    
    public static Number Parse(string s, IFormatProvider provider) {
        return double.Parse(s, provider);
    }
    
    public static bool TryParse(string s, out Number result) {
        result = 0;
        bool suc = double.TryParse(s, out double val);
        if (!suc) return false;
        result = val;
        return true;
    }
    
    public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Number result) {
        result = 0;
        bool suc = double.TryParse(s, style, provider, out double val);
        if (!suc) return false;
        result = val;
        return true;
    }
    
    public static Vector2 ToVector2(Number x, Number y) {
        return new Vector2(x, y);
    }
    
    public static Vector3 ToVector3(Number x, Number y, Number z) {
        return new Vector3(x, y, z);
    }
    
    public static Vector4 ToVector4(Number x, Number y, Number z, Number w) {
        return new Vector4(x, y, z, w);
    }
    
    public static Quaternion ToQuaternion(Number x, Number y, Number z, Number w) {
        return new Quaternion(x, y, z, w);
    }
    
    public static Matrix3x2 ToMatrix3x2(Number m11, Number m12, Number m21, Number m22, Number m31, Number m32) {
        return new Matrix3x2(m11, m12, m21, m22, m31, m32);
    }
    
    public static Matrix4x4 ToMatrix4x4(Number m11, Number m12, Number m13, Number m14, Number m21, Number m22, Number m23, Number m24, Number m31, Number m32, Number m33, Number m34, Number m41, Number m42, Number m43, Number m44) {
        return new Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
    }
    
    public static Number Pow(Number x, Number y) {
        return Math.Pow(x, y);
    }
    
    public static Number Abs(Number x) {
        return Math.Abs(x);
    }
    
    public static Number Acos(Number x) {
        return Math.Acos(x);
    }
    
    public static Number Acosh(Number x) {
        return Math.Acosh(x);
    }
    
    public static Number Asin(Number x) {
        return Math.Asin(x);
    }
    
    public static Number Asinh(Number x) {
        return Math.Asinh(x);
    }
    
    public static Number Atan(Number x) {
        return Math.Atan(x);
    }
    
    public static Number Atan2(Number y, Number x) {
        return Math.Atan2(y, x);
    }
    
    public static Number Atanh(Number x) {
        return Math.Atanh(x);
    }
    
    public static Number Cbrt(Number x) {
        return Math.Cbrt(x);
    }
    
    public static Number Ceiling(Number x) {
        return Math.Ceiling(x.DoubleValue);
    }
    
    public static Number Cos(Number x) {
        return Math.Cos(x);
    }
    
    public static Number Cosh(Number x) {
        return Math.Cosh(x);
    }
    
    public static Number Exp(Number x) {
        return Math.Exp(x);
    }
    
    public static Number Floor(Number x) {
        return Math.Floor(x.DoubleValue);
    }
    
    public static Number Log(Number x) {
        return Math.Log(x);
    }
    
    public static Number Log10(Number x) {
        return Math.Log10(x);
    }
    
    public static Number Log2(Number x) {
        return Math.Log(x, 2);
    }
    
    public static Number Max(Number a, Number b) {
        return Math.Max(a, b);
    }
    
    public static Number Min(Number a, Number b) {
        return Math.Min(a, b);
    }

    public static Number Round(Number x) {
        return Math.Round(x.DoubleValue);
    }

}