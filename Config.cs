using System.Globalization;
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
            
            // Create default config with strings only
            Dictionary<string, string> defaultConfig = new();
            foreach ((string key, Property value) in defaultValues) {
                switch (value.Type) {
                    case Property.PropertyType.String:
                        defaultConfig.Add(key, value);
                        break;
                    case Property.PropertyType.Integer:
                        defaultConfig.Add(key, value.Integer.ToString());
                        break;
                    case Property.PropertyType.Decimal:
                        defaultConfig.Add(key, value.Decimal.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Property.PropertyType.Float:
                        defaultConfig.Add(key, value.Float.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Property.PropertyType.Boolean:
                        defaultConfig.Add(key, value.Boolean.ToString());
                        break;
                    case Property.PropertyType.Date:
                        defaultConfig.Add(key, $"DATETIME{value.Date.ToBinary()}");
                        break;
                    case Property.PropertyType.Null:
                        defaultConfig.Add(key, "");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            File.Create(file).Close();
            File.WriteAllText(file, JsonSerializer.Serialize(defaultConfig, _serializerOptions));
            LogInfo("Config file created with default values");
            Values = defaultValues;
            return;
        }
        // Get config data
        string data = File.ReadAllText(file);

        Dictionary<string, string>? configDict;
        try {
            configDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
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
            switch (defaultValues[requiredValue].Type) {
                case Property.PropertyType.String:
                    configDict.Add(requiredValue, defaultValues[requiredValue]);
                    break;
                case Property.PropertyType.Integer:
                    configDict.Add(requiredValue, defaultValues[requiredValue].Integer.ToString());
                    break;
                case Property.PropertyType.Decimal:
                    configDict.Add(requiredValue, defaultValues[requiredValue].Decimal.ToString(CultureInfo.InvariantCulture));
                    break;
                case Property.PropertyType.Float:
                    configDict.Add(requiredValue, defaultValues[requiredValue].Float.ToString(CultureInfo.InvariantCulture));
                    break;
                case Property.PropertyType.Boolean:
                    configDict.Add(requiredValue, defaultValues[requiredValue].Boolean.ToString());
                    break;
                case Property.PropertyType.Date:
                    configDict.Add(requiredValue, $"DATETIME{defaultValues[requiredValue].Date.ToBinary()}");
                    break;
                case Property.PropertyType.Null:
                    configDict.Add(requiredValue, "");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //configDict.Add(requiredValue, defaultValues[requiredValue]);
            LogInfo($"Config file is missing required value ({requiredValue}) and was added with " +
                    $"default value ({defaultValues[requiredValue]})");
            wholeConfigValid = false;
        }
        if (!wholeConfigValid) {
            // Save the config file
            File.WriteAllText(file, JsonSerializer.Serialize(configDict, _serializerOptions));
            LogInfo("Wrote missing config values to config file");
        }
        
        Dictionary<string, Property> convertedConfig = new();

        foreach (KeyValuePair<string, string> configKvp in configDict) {
            
            // Is it null?
            if (configKvp.Value == "") {
                convertedConfig.Add(configKvp.Key, new Property());
                continue;
            }
            
            // Is it a date?
            if (configKvp.Value.StartsWith("DATETIME")) {
                string datetimeBinary = configKvp.Value.Replace("DATETIME", "");
                if (long.TryParse(datetimeBinary, out long datetimeBinLong)) {
                    convertedConfig.Add(configKvp.Key, DateTime.FromBinary(datetimeBinLong));
                    continue;
                }
            }
            
            // Is it a boolean?
            if (configKvp.Value is "True" or "False") {
                convertedConfig.Add(configKvp.Key, configKvp.Value == "True");
                continue;
            }
            
            // Is it an integer?
            if (int.TryParse(configKvp.Value, out int intVal)) {
                convertedConfig.Add(configKvp.Key, intVal);
                continue;
            }
            
            // Is it a decimal?
            if (decimal.TryParse(configKvp.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalVal)) {
                convertedConfig.Add(configKvp.Key, (double)decimalVal);
                continue;
            }
            
            // Is it a float?
            if (float.TryParse(configKvp.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatVal)) {
                convertedConfig.Add(configKvp.Key, floatVal);
                continue;
            }
            
            // It's a string
            convertedConfig.Add(configKvp.Key, configKvp.Value);
        }

        // Return the patched config
        Values = convertedConfig;
    }
    
}