using System.Text.Json;

namespace GeneralPurposeLib; 

public class Jsonable {
    private readonly JsonElement _data;

    public Jsonable(string json) {
        _data = JsonSerializer.Deserialize<JsonElement>(json);
    }

    public Jsonable(ReadOnlySpan<byte> json) {
        _data = JsonSerializer.Deserialize<JsonElement>(json);
    }

    public T Get<T>(string path) {
        string[] dirs = path.Split('/');
        JsonElement next = dirs.Aggregate(_data, (current, dir) => current.GetProperty(dir));
        return next.Deserialize<T>()!;
    }

    public T? GetOrDefault<T>(string path, T defaultValue) {
        return TryGet(path, out T? value) ? value : defaultValue;
    }

    public bool TryGet<T>(string path, out T? value) {
        try {
            value = Get<T>(path);
            return true;
        }
        catch {
            value = default;
            return false;
        }
    }
}