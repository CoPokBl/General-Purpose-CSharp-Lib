namespace GeneralPurposeLib; 

public static class GlobalConfig {

    private static Config? _config;
    public static Config Config {
        get {
            if (_config == null) {
                throw new Exception("Config not initialized");
            }
            return _config;
        }
    }

    public static void Init(Config config) {
        _config = config;
    }
    
}