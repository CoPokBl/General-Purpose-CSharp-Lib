using System.Text.Json;

namespace GeneralPurposeLib; 

public class Config {
    
    private Dictionary<string, Property> Values { get; set; }
    
    private readonly JsonSerializerOptions _serializerOptions = new() {
        WriteIndented = true
    };

    private readonly bool _log;
    
    public Property this[string key] {
        get => Values[key];
        set => Values[key] = value;
    }
    
    public bool ContainsKey(string key) => Values.ContainsKey(key);
    public bool ContainsValue(Property value) => Values.ContainsValue(value);

    private void LogInfo(object msg) {
        if (!_log) { return; }
        Logger.Info(msg);
    }
    
    private void LogDebug(object msg) {
        if (!_log) { return; }
        Logger.Info(msg);
    }

    public Config(Dictionary<string, Property> defaultValues, string file = "config.json", bool log = true) {
        _log = log;
        
        if (!File.Exists(file)) {
            // It doesn't exist, so create it and give them the default config
            File.Create(file).Close();
            File.WriteAllText(file, JsonSerializer.Serialize(defaultValues, _serializerOptions));
            LogInfo("Config file created with default values");
            Values = defaultValues;
            return;
        }
        // Get config data
        string data = File.ReadAllText(file);

        Dictionary<string, object>? configDict;
        try {
            configDict = JsonSerializer.Deserialize<Dictionary<string, object>>(data);
            if (configDict == null) { throw new InvalidConfigException("Config file is not valid JSON"); }
        }
        catch (Exception e) {
            // Config is invalid
            LogDebug(e);
            throw new InvalidConfigException("Config file is invalid: " + e.Message);
        }
            
        // Check if all the required values are there
        bool wholeConfigValid = true;
        foreach (string requiredValue in defaultValues.Keys.ToArray()) {
            if (configDict.ContainsKey(requiredValue)) { continue; }
            
            // Missing a required value, so add it
            configDict.Add(requiredValue, defaultValues[requiredValue]);
            LogInfo($"Config file is missing required value ({requiredValue}) and was added with " +
                    $"default value ({defaultValues[requiredValue]})");
            wholeConfigValid = false;
        }
        if (!wholeConfigValid) {
            // Save the config file
            File.WriteAllText(file, JsonSerializer.Serialize(configDict, _serializerOptions));
            LogInfo("Wrote missing config values to config file");
        }
        
        // Return the patched config
        Values = configDict.ToDictionary(x => x.Key, x => new Property(x.Value));
    }
    
}