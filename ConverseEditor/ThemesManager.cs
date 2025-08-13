using Hexa.NET.ImGui;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ConverseEditor;

public class ThemesManager
{
    static readonly ThemesManager instance = new();
    public static ThemesManager Instance { get { return instance; } }

    public Dictionary<string, JsonDocument> Themes = new();

    public ThemesManager() => Init();

    void Init()
    {
        Load();

        SetTheme(SettingsManager.Instance.settings.SelectedTheme);
    }

    void Load()
    {
        if (!Directory.Exists("themes")) return;

        Themes.Clear();

        foreach (var i in Directory.GetFiles("themes"))
            Themes.Add(Path.GetFileNameWithoutExtension(i), JsonDocument.Parse(File.ReadAllText(i)));
    }

    public void SetTheme(string theme)
    {
        if (!Themes.ContainsKey(theme)) return;

        ImGui.StyleColorsDark();
        var style = ImGui.GetStyle();
        var doc = Themes[theme];
        var root = doc.RootElement.GetProperty("Style");

        var styleType = typeof(ImGuiStyle);
        var fields = styleType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        unsafe
        {
            foreach (var prop in root.EnumerateObject())
            {
                if (prop.NameEquals("Colors")) continue;

                var field = typeof(ImGuiStyle).GetField(prop.Name, BindingFlags.Public | BindingFlags.Instance);
                if (field == null) continue;

                var fieldType = field.FieldType;

                TypedReference tr = __makeref(style);
                IntPtr stylePtr = **(IntPtr**)(&tr);

                int offset = (int)Marshal.OffsetOf(typeof(ImGuiStyle), field.Name);

                byte* basePtr = (byte*)stylePtr.ToPointer();

                if (fieldType == typeof(float))
                {
                    float value = prop.Value.GetSingle();
                    float* fieldPtr = (float*)(basePtr + offset);
                    *fieldPtr = value;
                }
                else if (fieldType == typeof(Vector2))
                {
                    var arr = prop.Value.EnumerateArray().ToArray();
                    Vector2 vec = new Vector2(arr[0].GetSingle(), arr[1].GetSingle());
                    Vector2* vecPtr = (Vector2*)(basePtr + offset);
                    *vecPtr = vec;
                }
                else if (fieldType.IsEnum)
                {
                    object enumValue = null;

                    if (prop.Value.ValueKind == JsonValueKind.String)
                    {
                        string enumName = prop.Value.GetString();
                        try
                        {
                            enumValue = Enum.Parse(fieldType, enumName);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else if (prop.Value.ValueKind == JsonValueKind.Number)
                    {
                        var underlyingType = Enum.GetUnderlyingType(fieldType);

                        if (underlyingType == typeof(int))
                            enumValue = Enum.ToObject(fieldType, prop.Value.GetInt32());
                        else if (underlyingType == typeof(byte))
                            enumValue = Enum.ToObject(fieldType, prop.Value.GetByte());
                        else if (underlyingType == typeof(short))
                            enumValue = Enum.ToObject(fieldType, prop.Value.GetInt16());
                        else if (underlyingType == typeof(long))
                            enumValue = Enum.ToObject(fieldType, prop.Value.GetInt64());
                        else continue;
                    }
                    else continue;

                    int enumInt = Convert.ToInt32(enumValue);
                    int* fieldPtr = (int*)(basePtr + offset);
                    *fieldPtr = enumInt;
                }
            }
        }

        if (root.TryGetProperty("Colors", out var colors) && colors.ValueKind == JsonValueKind.Object)
        {
            foreach (var colorProp in colors.EnumerateObject())
            {
                if (Enum.TryParse<ImGuiCol>(colorProp.Name, out var col))
                {
                    var colorArray = colorProp.Value.EnumerateArray().Select(x => x.GetSingle()).ToArray();
                    if (colorArray.Length == 4)
                        style.Colors[(int)col] = new(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                }
            }
        }
    }
}
