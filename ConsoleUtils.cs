using System.Text;

namespace GeneralPurposeLib; 

public static class ConsoleUtils {
    
    public static string CustomInput(bool enableMask = false, char mask = '*') {
        StringBuilder pass = new ();
        
        while (true) {
            ConsoleKeyInfo ki = Console.ReadKey(true);

            if (ki.Key == ConsoleKey.Enter) break;
            if (ki.Key == ConsoleKey.Backspace) {
                if (pass.Length < 1) continue;

                pass.Remove(pass.Length - 1, 1);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                continue;
            }

            pass.Append(ki.KeyChar);
            Console.Write(enableMask ? mask : ki.KeyChar);
        }
        
        Console.Write("\n");
        return pass.ToString();
    }

    public static string PasswordInput(string? prompt = null, char mask = '*') {
        if (prompt != null) {
            Console.Write(prompt);
        }
        return CustomInput(true, mask);
    }

    public static string? TextInput(string? prompt = null) {
        if (prompt != null) {
            Console.Write(prompt);
        }
        return Console.ReadLine();
    }

}