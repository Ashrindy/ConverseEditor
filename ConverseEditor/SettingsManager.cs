using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConverseEditor;

public class SettingsManager
{
    static readonly SettingsManager instance = new();
    public static SettingsManager Instance { get { return instance; } }

    public struct Settings
    {
        [JsonPropertyName("selectedTheme"), JsonPropertyOrder(1)]
        public string SelectedTheme { get; set; } = "Light";

        public Settings() { }
    }

    public readonly string SettingsFilename = "settings.json";
    public Settings settings = new();

    public SettingsManager()
    {
        if (File.Exists(SettingsFilename)) Load();
        else Save();
    }

    public void Load() => Read(SettingsFilename);
    void Read(string filepath) => settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(filepath));

    public void Save() => Write(SettingsFilename);
    void Write(string filepath) => File.WriteAllText(filepath, JsonSerializer.Serialize(settings));
}
