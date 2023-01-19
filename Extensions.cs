using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GeneralPurposeLib; 

public static class Extensions {

    public static void WaitUntilCancel(this CancellationToken token) {
        try {
            Task.Delay(-1, token).RunTaskSync();
        }
        catch (TaskCanceledException) {
            // Ignore and return
        }
    }
    
    public static void WaitUntilCancel(this CancellationTokenSource token) {
        token.Token.WaitUntilCancel();
    }
    
    /// <summary>
    /// Formats a TimeSpan object into a string with a custom format
    /// </summary>
    /// <param name="timeSpan">The TimeSpan object to format</param>
    /// <param name="format">
    /// The format for the string, {ms} is milliseconds, {s} is seconds, {m} is minutes, {h} is hours, {d} is days
    /// </param>
    /// <returns>A string containing the formatted TimeSpan</returns>
    public static string ToFormat(this TimeSpan timeSpan, string format) {
        string result = format;
        result = result.Replace("{d}", timeSpan.Days.ToString());
        result = result.Replace("{h}", timeSpan.Hours.ToString());
        result = result.Replace("{m}", timeSpan.Minutes.ToString());
        result = result.Replace("{s}", timeSpan.Seconds.ToString());
        result = result.Replace("{ms}", timeSpan.Milliseconds.ToString());
        return result;
    }
    
    /// <summary>
    /// Runs a task, waits for it to finish, and returns the result
    /// </summary>
    /// <param name="task">The task to run</param>
    /// <typeparam name="T">The return type of the task</typeparam>
    /// <returns>The task's result</returns>
    public static T RunTaskSync<T>(this Task<T> task) {
        while (!task.IsCompleted) {
            // Just wait
        }
        return task.Result;
    }
    
    /// <summary>
    /// Runs a task and waits for it to finish
    /// </summary>
    /// <param name="task">The task to run</param>
    public static void RunTaskSync(this Task task) {
        while (!task.IsCompleted) {
            // Just wait
        }
    }
    
    /// <summary>
    /// Waits until obj == value
    /// </summary>
    /// <param name="obj">A reference of the object to check</param>
    /// <param name="value">The value that we should wait for obj to equal</param>
    public static void WaitUntilEquals(this ref bool obj, bool value) {
        while (obj != value) {
            Thread.Sleep(1);
        }
    }
    
    /// <summary>
    /// Waits until obj == value
    /// </summary>
    /// <param name="obj">A reference of the object to check</param>
    /// <param name="value">The value that we should wait for obj to equal</param>
    public static void WaitUntilEquals(this ref int obj, int value) {
        while (obj != value) {
            Thread.Sleep(1);
        }
    }
    
    /// <summary>
    /// Waits until obj == value
    /// </summary>
    /// <param name="obj">A reference of the object to check</param>
    /// <param name="value">The value that we should wait for obj to equal</param>
    public static void WaitUntilEquals(this ref float obj, float value) {
        while (Math.Abs(obj - value) > 0.1) {
            Thread.Sleep(1);
        }
    }
    
    /// <summary>
    /// Waits until obj == value
    /// </summary>
    /// <param name="obj">A reference of the object to check</param>
    /// <param name="value">The value that we should wait for obj to equal</param>
    public static void WaitUntilEquals(this ref double obj, double value) {
        while (Math.Abs(obj - value) > 0.1) {
            Thread.Sleep(1);
        }
    }

    /// <summary>
    /// When the Console.CancelKeyPress event is raised, cancel it and then cancel the given CancellationTokenSource
    /// </summary>
    /// <param name="source">The CancellationTokenSource to cancel</param>
    public static void CancelOnCancelKeyPress(this CancellationTokenSource source) {
        Console.CancelKeyPress += (_, args) => {
            source.Cancel();
            args.Cancel = true;
        };
    }

    /// <summary>
    /// Hash the specified string using MD5
    /// </summary>
    /// <param name="str">The string to hash</param>
    /// <returns>The MD5 hash of the specified string</returns>
    public static string Md5Hash(this string str) {
        StringBuilder builder = new StringBuilder();
        foreach (byte t in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str))) {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Serializes an object to a JSON string using the System.Text.Json library
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <returns>A JSON string representing the object</returns>
    public static string ToJson(this object obj) {
        return JsonSerializer.Serialize(obj);
    }
    
    /// <summary>
    /// Deserializes a JSON string to an object using the System.Text.Json library
    /// </summary>
    /// <param name="json">The JSON string</param>
    /// <typeparam name="T">The target deserialization type</typeparam>
    /// <returns>The deserialized object</returns>
    public static T? FromJson<T>(this string json) {
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Deserializes a JSON string to an object using the System.Text.Json library
    /// </summary>
    /// <param name="json">The JSON string</param>
    /// <param name="defaultValue">The default value for deserialization</param>
    /// <typeparam name="T">The target deserialization type</typeparam>
    /// <returns>The deserialized object if successful otherwise the default value</returns>
    public static T FromJson<T>(this string json, T defaultValue) {
        try {
            T? val = JsonSerializer.Deserialize<T>(json);
            return val ?? defaultValue;
        } catch {
            return defaultValue;
        }
    }

    /// <summary>
    /// Deserializes a JSON string to an object using the System.Text.Json library
    /// </summary>
    /// <param name="json">The JSON string</param>
    /// <param name="defaultValue">The default value for deserialization</param>
    /// <param name="options">Custom JSON serialization settings</param>
    /// <typeparam name="T">The target deserialization type</typeparam>
    /// <returns>The deserialized object if successful otherwise the default value</returns>
    public static T FromJson<T>(this string json, T defaultValue, JsonSerializerOptions options) {
        try {
            T? val = JsonSerializer.Deserialize<T>(json, options);
            return val ?? defaultValue;
        } catch {
            return defaultValue;
        }
    }
    
    /// <summary>
    /// Checks whether an object is null
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <returns>Whether or not the object is null</returns>
    public static bool IsNull(this object? obj) {
        return obj == null;
    }
    
    /// <summary>
    /// Throws an error if the object is null otherwise returns the original object without nullability
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <param name="exception">The exception to throw, defaults to NullReferenceException</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>A non nullable version of the object</returns>
    /// <exception cref="Exception">Throws when the specified object is null</exception>
    public static T ThrowIfNull<T>(this T? obj, Exception? exception = null) {
        if (obj == null) {
            throw exception ?? new NullReferenceException("Object is null");
        }
        return obj;
    }
    
    /// <summary>
    /// Throws an error if the object is null otherwise returns the original object without nullability
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <param name="exception">The exception to throw, defaults to NullReferenceException</param>
    /// <returns>A non nullable version of the object</returns>
    /// <exception cref="Exception">Throws when the specified object is null</exception>
    public static object ThrowIfNull(this object? obj, Exception? exception = null) {
        if (obj == null) {
            throw exception ?? new NullReferenceException("Object is null");
        }
        return obj;
    }
    
    /// <summary>
    /// If the object is null, returns the default value for the type, otherwise returns the original object
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <param name="defaultValue">The value to return upon the object being null</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>The object or the default value if the object is null</returns>
    public static T DefaultIfNull<T>(this T? obj, T defaultValue) where T : struct {
        return obj ?? defaultValue;
    }
    
    /// <summary>
    /// If the object is null, returns the default value for the type, otherwise returns the original object
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <param name="defaultValue">The value to return upon the object being null</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>The object or the default value if the object is null</returns>
    public static T DefaultIfNull<T>(this T? obj, T defaultValue) where T : class {
        return obj ?? defaultValue;
    }

    /// <summary>
    /// Executes the specified function with the object as the parameter and returns the function's result
    /// </summary>
    /// <param name="obj">The object to supply as input</param>
    /// <param name="func">The function to execute, must take T and return T</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>The result of the specified function</returns>
    public static T Mutate<T>(this T obj, Func<T, T> func) {
        return func.Invoke(obj);
    }

    /// <summary>
    /// Executes the specified function with the object as the parameter and returns the function's result
    /// </summary>
    /// <param name="obj">The object to supply as input</param>
    /// <param name="func">The function to execute, must take T and return TO</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <typeparam name="TO">The return type for the function</typeparam>
    /// <returns>The result of the specified function</returns>
    public static TO Run<T, TO>(this T obj, Func<T, TO> func) {
        return func.Invoke(obj);
    }
    
    /// <summary>
    /// Executes the specified function with the object as the parameter and returns the function's result or the
    /// original object if the function throws an exception
    /// </summary>
    /// <param name="obj">The object to supply as input</param>
    /// <param name="func">The function to execute, must take T and return T</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>The result of the specified function or the original value if the function threw an error</returns>
    public static T RunIgnoreErrors<T>(this T obj, Func<T, T> func) {
        try {
            return func.Invoke(obj);
        } catch {
            return obj;
        }
    }

    /// <summary>
    /// Executes the specified function with the object as the parameter and returns the function's result
    /// or the default value if the function throws an exception
    /// </summary>
    /// <param name="obj">The object to supply as input</param>
    /// <param name="func">The function to execute, must take T and return T</param>
    /// <param name="defaultValue">The value to return if the function throws an error</param>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <returns>The result of the specified function or the default value if the function threw an error</returns>
    public static T RunOrDefaultOnError<T>(this T obj, Func<T, T> func, T defaultValue) {
        try {
            return func.Invoke(obj);
        } catch {
            return defaultValue;
        }
    }

    /// <summary>
    /// Converts the boolean to an integer (1 or 0)
    /// </summary>
    /// <param name="boolean">The boolean to convert</param>
    /// <returns>1 is the boolean is true otherwise 0</returns>
    public static int ToInt(this bool boolean) {
        return boolean ? 1 : 0;
    }
    
    /// <summary>
    /// Converts the integer to a boolean (1 is true)
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>True if the integer is 1 otherwise false</returns>
    public static bool ToBoolean(this int integer) {
        return integer == 1;
    }
    
    /// <summary>
    /// Converts the integer to a boolean (1 is true)
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>True if the integer is 1 otherwise false</returns>
    public static bool ToBoolean(this long integer) {
        return integer == 1;
    }
    
    /// <summary>
    /// Converts the integer to a boolean (1 is true)
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>True if the integer is 1 otherwise false</returns>
    public static bool ToBoolean(this short integer) {
        return integer == 1;
    }
    
    /// <summary>
    /// Converts the integer to a boolean (1 is true)
    /// </summary>
    /// <param name="integer">The integer to convert</param>
    /// <returns>True if the integer is 1 otherwise false</returns>
    public static bool ToBoolean(this byte integer) {
        return integer == 1;
    }
    
    /// <summary>
    /// Computes a SHA256 hash of the string
    /// </summary>
    /// <param name="input">The string to compute a hash for</param>
    /// <returns>The generated hash</returns>
    public static string Sha256Hash(this string input) {
        return Utils.ToSHA256(input);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this int value) {
        return new Number(value);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this long value) {
        return new Number(value);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this short value) {
        return new Number(value);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this byte value) {
        return new Number(value);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this float value) {
        return new Number(value);
    }
    
    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this double value) {
        return new Number(value);
    }

    /// <summary>
    /// Converts the number to a Number object
    /// </summary>
    /// <param name="value">The number</param>
    /// <returns>A Number object containing the input</returns>
    public static Number ToNumber(this decimal value) {
        return new Number((double)value);
    }

}