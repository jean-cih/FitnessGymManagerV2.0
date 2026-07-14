using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using NAudio.Wave;

public static class ConfigManager
{
    private static string filePath;
    private static Dictionary<string, object> settings;

    public static void Initialize(string path)
    {
        filePath = path;
        LoadSettings();
    }

    private static void LoadSettings()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                CreateDefaultSettings();
                return;
            }

            string jsonString = File.ReadAllText(filePath, Encoding.UTF8);

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                settings = new Dictionary<string, object>();

                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    settings[property.Name] = ConvertJsonElement(property.Value);
                }
            }

            if (settings == null || settings.Count == 0)
                CreateDefaultSettings();
        }
        catch
        {
            CreateDefaultSettings();
        }
    }

    private static object ConvertJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                return element.TryGetInt32(out int intValue) ? intValue : element.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
                return null;
            case JsonValueKind.Object:
                return element.Clone();
            case JsonValueKind.Array:
                return element.Clone();
            default:
                return element.Clone();
        }
    }

    private static void CreateDefaultSettings()
    {
        settings = new Dictionary<string, object>
        {
            ["headlineSize"] = 10,
            ["sizeKeyName"] = 10,
            ["sizeTableTitle"] = 10,
            ["textSize"] = 10,
            ["designForm"] = "None",
            ["designBackground"] = "Casual"
        };
        SaveSettings();
    }

    public static void CreateFile()
    {
        CreateDefaultSettings();
    }

    public static void UpdateSetting(string key, object value)
    {
        if (settings.ContainsKey(key))
        {
            if (value is JsonElement jsonElement)
            {
                settings[key] = ConvertJsonElement(jsonElement);
            }
            else
            {
                settings[key] = value;
            }
            SaveSettings();
        }
        else
        {
            throw new KeyNotFoundException($"Ключ '{key}' не найден");
        }
    }

    public static T GetSetting<T>(string key)
    {
        if (!settings.ContainsKey(key))
            throw new KeyNotFoundException($"Ключ '{key}' не найден");

        object value = settings[key];

        if (value is T typedValue)
            return typedValue;

        if (typeof(T) == typeof(string))
            return (T)(object)(value?.ToString() ?? string.Empty);

        if (typeof(T) == typeof(int))
        {
            if (value is string stringValue && int.TryParse(stringValue, out int intResult))
                return (T)(object)intResult;
            return (T)(object)Convert.ToInt32(value);
        }

        if (typeof(T) == typeof(double))
        {
            if (value is string stringValue && double.TryParse(stringValue, out double doubleResult))
                return (T)(object)doubleResult;
            return (T)(object)Convert.ToDouble(value);
        }

        if (typeof(T) == typeof(bool))
        {
            if (value is string stringValue && bool.TryParse(stringValue, out bool boolResult))
                return (T)(object)boolResult;
            return (T)(object)Convert.ToBoolean(value);
        }

        try
        {
            string jsonValue = JsonSerializer.Serialize(value);
            return JsonSerializer.Deserialize<T>(jsonValue);
        }
        catch (Exception ex)
        {
            throw new InvalidCastException(
                $"Не удалось преобразовать значение '{value}' в тип {typeof(T).Name}",
                ex
            );
        }
    }

    private static void SaveSettings()
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, jsonString, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }
}