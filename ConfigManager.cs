using System.Text.Json;

namespace GeneralPurposeLib; 

public class ConfigManager {
    public string ConfigFileName { get; set; } = "config.json";

    /// <summary>
    /// The default options in the config file.
    /// </summary>
    private readonly Dictionary<string, string> _defaultConfig = new();

    /// <summary>
    /// The options for the serializer.
    /// </summary>
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions() {
        WriteIndented = true
    };
    
    // All the values that should be in the config file
    private string[] RequiredConfigValues => _defaultConfig.Keys.ToArray();
    
    // Constructor
    public ConfigManager(string file = "config.json", Dictionary<string, string>? defaultConf = null, JsonSerializerOptions? serializerOptions = null) {
        ConfigFileName = file;
        _defaultConfig = defaultConf ?? _defaultConfig;
        _serializerOptions = serializerOptions ?? _serializerOptions;
    }

    /// <summary>
    /// Loads the config file and returns a dictionary of the values
    /// </summary>s
    /// <returns>The config file represented as a Dictionary</returns>
    public Dictionary<string, string> LoadConfig() {
        
        // Don't bother creating a default config because they get it when they build
        if (!File.Exists(ConfigFileName)) {
            // It doesn't exist, so create it and give them the default config
            File.Create(ConfigFileName).Close();
            File.WriteAllText(ConfigFileName, JsonSerializer.Serialize(_defaultConfig, _serializerOptions));
            Logger.Info("Config file created with default values");
            return _defaultConfig;
        }
        // Get config data
        string data = File.ReadAllText(ConfigFileName);

        Dictionary<string, string> configDict;
        try {
            configDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
            if (configDict == null) { throw new InvalidConfigException("Config file is not valid JSON"); }
        }
        catch (Exception e) {
            // Config is invalid
            Logger.Debug(e);
            throw new InvalidConfigException("Config file is invalid: " + e.Message);
        }
            
        // Check if all the required values are there
        bool wholeConfigValid = true;
        foreach (string requiredValue in RequiredConfigValues) {
            if (configDict.ContainsKey(requiredValue)) { continue; }
            
            // Missing a required value, so add it
            configDict.Add(requiredValue, _defaultConfig[requiredValue]);
            Logger.Info($"Config file is missing required value ({requiredValue}) and was added with " +
                        $"default value ({_defaultConfig[requiredValue]})");
            wholeConfigValid = false;
        }
        if (!wholeConfigValid) {
            // Save the config file
            File.WriteAllText(ConfigFileName, JsonSerializer.Serialize(configDict, _serializerOptions));
            Logger.Info("Wrote missing config values to config file");
        }
            
        // Return the patched config
        return configDict;
            
    }

}

public class InvalidConfigException : Exception {
    public InvalidConfigException(string message) : base(message) { }
}
