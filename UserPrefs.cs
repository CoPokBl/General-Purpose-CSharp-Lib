using System.Text.Json;

namespace GeneralPurposeLib; 

public static class UserPrefs {
    
    private const string FileLocation = "UserPrefs.json";
    private static Dictionary<string, string>? _prefs;

    /// <summary>
    /// Gets a stored preference.
    /// </summary>
    /// <param name="key">The key of the value to obtain.</param>
    /// <returns>The requested value or null if not found.</returns>
    public static string? GetString(string key) {
        if (_prefs == null) {
            // Load prefs from disk
            Load();
        }

        return !_prefs!.ContainsKey(key) ? null : _prefs[key];
    }

    /// <summary>
    /// Gets a stored preference.
    /// </summary>
    /// <param name="key">The key of the value to obtain.</param>
    /// <param name="defaultValue">The value to return if the specified key was not found.</param>
    /// <returns>The requested value or the default value if it was not fun.</returns>
    public static string GetString(string key, string defaultValue) => GetString(key) ?? defaultValue;
    
    /// <summary>
    /// Sets a preference.
    /// WARNING: Use Save(); to save the preference to disk.
    /// </summary>
    /// <param name="key">The key to store.</param>
    /// <param name="value">The value to store.</param>
    public static void SetString(string key, string value) {
        if (_prefs == null) {
            // Load prefs from disk
            Load();
        }

        _prefs![key] = value;
    }
    
    // Load prefs function
    private static void Load() {
        if (!File.Exists(FileLocation)) {
            _prefs = new Dictionary<string, string>();
            return;
        }
        
        // Load prefs from disk
        _prefs = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(FileLocation));
        
    }
    
    /// <summary>
    /// Save prefs to disk.
    /// </summary>
    public static void Save() {
        if (_prefs == null) {
            // Nothing to save
            return;
        }

        // Save prefs to disk
        File.WriteAllText(FileLocation, JsonSerializer.Serialize(_prefs));
    }
    
}