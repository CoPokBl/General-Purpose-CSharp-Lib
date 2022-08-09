using System.Text.Json;

namespace GeneralPurposeLib; 

public static class Extensions {
    
    public static string ToFormat(this TimeSpan timeSpan, string format) {
        string result = format;
        result = result.Replace("{d}", timeSpan.Days.ToString());
        result = result.Replace("{h}", timeSpan.Hours.ToString());
        result = result.Replace("{m}", timeSpan.Minutes.ToString());
        result = result.Replace("{s}", timeSpan.Seconds.ToString());
        result = result.Replace("{ms}", timeSpan.Milliseconds.ToString());
        return result;
    }

    public static string ToJson(this object obj) {
        return JsonSerializer.Serialize(obj);
    }
    
    public static T? FromJson<T>(this string json) {
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static T FromJson<T>(this string json, T defaultValue) {
        try {
            T? val = JsonSerializer.Deserialize<T>(json);
            return val ?? defaultValue;
        } catch {
            return defaultValue;
        }
    }
    
    public static T FromJson<T>(this string json, T defaultValue, JsonSerializerOptions options) {
        try {
            T? val = JsonSerializer.Deserialize<T>(json, options);
            return val ?? defaultValue;
        } catch {
            return defaultValue;
        }
    }
    
    public static bool IsNull(this object? obj) {
        return obj == null;
    }
    
    public static T ThrowIfNull<T>(this T? obj, Exception? exception = null) {
        if (obj == null) {
            throw exception ?? new NullReferenceException("Object is null");
        }
        return obj;
    }
    
    public static object ThrowIfNull(this object? obj, Exception? exception = null) {
        if (obj == null) {
            throw exception ?? new NullReferenceException("Object is null");
        }
        return obj;
    }
    
    public static T DefaultIfNull<T>(this T? obj, T defaultValue) where T : struct {
        return obj ?? defaultValue;
    }
    
    public static T DefaultIfNull<T>(this T? obj, T defaultValue) where T : class {
        return obj ?? defaultValue;
    }

    public static T Mutate<T>(this T obj, Func<T, T> func) {
        return func.Invoke(obj);
    }
    
    public static TO Run<T, TO>(this T obj, Func<T, TO> func) {
        return func.Invoke(obj);
    }
    
    public static T RunIgnoreErrors<T>(this T obj, Func<T, T> func) {
        try {
            return func.Invoke(obj);
        } catch {
            return obj;
        }
    }
    
    public static T RunOrDefaultOnError<T>(this T obj, Func<T, T> func, T defaultValue) {
        try {
            return func.Invoke(obj);
        } catch {
            return defaultValue;
        }
    }

    public static int ToInt(this bool boolean) {
        return boolean ? 1 : 0;
    }
    
    public static bool ToBoolean(this int integer) {
        return integer == 1;
    }
    
    public static bool ToBoolean(this long integer) {
        return integer == 1;
    }
    
    public static bool ToBoolean(this short integer) {
        return integer == 1;
    }
    
    public static bool ToBoolean(this byte integer) {
        return integer == 1;
    }
    
    public static string Sha256Hash(this string input) {
        return Utils.ToSHA256(input);
    }
    
    public static Number ToNumber(this int value) {
        return new Number(value);
    }
    
    public static Number ToNumber(this long value) {
        return new Number(value);
    }
    
    public static Number ToNumber(this short value) {
        return new Number(value);
    }
    
    public static Number ToNumber(this byte value) {
        return new Number(value);
    }
    
    public static Number ToNumber(this float value) {
        return new Number(value);
    }
    
    public static Number ToNumber(this double value) {
        return new Number(value);
    }

    public static Number ToNumber(this decimal value) {
        return new Number((double)value);
    }

}