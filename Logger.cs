/*
 
 Credit to Calcilore (https://github.com/Calcilore) for this file
 Original: https://github.com/Calcilore/RayKeys/blob/main/Misc/Logger.cs
 
 Modified by CoPokBl
*/

using System.IO.Compression;
using System.Reflection;

namespace GeneralPurposeLib; 

public static class Logger {
    public static LogLevel LoggingLevel { get; set; } = LogLevel.Debug;
    private static FileStream? _logFile;
    private static readonly string ExecDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
    private static StreamWriter? _streamWriter;
    private static Task _writeTask = Task.CompletedTask;
    private static string? _typeText;
    
    private static readonly Dictionary<LogLevel, ConsoleColor> Colors = new() {
        { LogLevel.Debug, ConsoleColor.Green  },
        { LogLevel.Info , ConsoleColor.White  },
        { LogLevel.Warn , ConsoleColor.Yellow },
        { LogLevel.Error, ConsoleColor.Red    }
    };
    
    public static void Log(object logObj, LogLevel level) {
        if (level == LogLevel.None) {
            throw new ArgumentException("Log level cannot be None.");
        }
        
        if (LoggingLevel < level) { return; }
        
        string log = $"[{DateTime.Now.ToLongTimeString()}] [{level}]: {logObj}\n";
        
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = Colors[level];
        Console.Write(log);
        Console.ForegroundColor = originalColor;
        
        _typeText += log;

        if (!_writeTask.IsCompleted) { return; }
        _writeTask = _streamWriter!.WriteAsync(_typeText);
        _typeText = "";
    }

    public static void WaitFlush() {
        _writeTask.Wait();

        _streamWriter!.Write(_typeText);
        _typeText = "";
    }

    public static void Init(LogLevel logLevel) {
        LoggingLevel = logLevel;

        string logPath = Path.Join(ExecDirectory, "Logs");

        if (!Directory.Exists(logPath)) { Directory.CreateDirectory(logPath); }
        
        // Compress existing latest.log
        if (File.Exists(logPath + "/latest.log")) {
            using FileStream originalFileStream = File.Open(logPath + "/latest.log", FileMode.Open);
            string gzFileLoc = new StreamReader(originalFileStream).ReadLine() ?? string.Empty;

            try {
                gzFileLoc = logPath + gzFileLoc[gzFileLoc.LastIndexOf('/')..] + ".gz";
            }
            catch (Exception) { // in case it cant find date of latest.log, make name have random value
                gzFileLoc = logPath + "/Unknown-" + 
                            (int)Math.Abs(new Random().Next()*1000000*Math.PI) + ".log.gz";
            }

            originalFileStream.Seek(0, SeekOrigin.Begin);

            using FileStream compressedFileStream = File.Create(gzFileLoc);
            using GZipStream compressor = new(compressedFileStream, CompressionMode.Compress);
            originalFileStream.CopyTo(compressor);
        }

        string logFileName = $"{DateTime.Now:yyyy-MM-dd}-";

        int i = 1;
        while (File.Exists($"{logPath}/{logFileName}i.log.gz")) { i++; }  // Get a unique number for the name

        logFileName += i + ".log";
            
        _logFile = File.OpenWrite(logPath + "/latest.log");
        _streamWriter = new StreamWriter(_logFile);
        _streamWriter.AutoFlush = true;
        _typeText = "";
        Info($"Logging to: Logs/{logFileName}");
    }
        
    public static void Error(object log) => Log(log, LogLevel.Error);
    public static void Warn(object log) => Log(log, LogLevel.Warn);
    public static void Info(object log) => Log(log, LogLevel.Info);
    public static void Debug(object log) => Log(log, LogLevel.Debug);
}

public enum LogLevel { None, Error, Warn, Info, Debug }