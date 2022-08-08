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

}