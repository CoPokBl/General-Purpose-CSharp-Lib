using System.Security.Cryptography;
using System.Text;

namespace GeneralPurposeLib; 

public static class Utils {
    
    public static string ToSHA256(string str) {
        StringBuilder builder = new StringBuilder();
        foreach (byte t in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str))) {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
    
    public static string ToMD5(string str) {
        StringBuilder builder = new StringBuilder();
        foreach (byte t in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str))) {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
    
    /// <summary>
    /// Waits until obj == value
    /// </summary>
    /// <param name="obj">A reference of the object to check</param>
    /// <param name="value">The value that we should wait for obj to equal</param>
    /// <typeparam name="T">The type of the object</typeparam>
    public static void WaitUntilEquals<T>(ref T obj, T value) {
        while (!obj!.Equals(value)) {
            Thread.Sleep(1);
        }
    }

}