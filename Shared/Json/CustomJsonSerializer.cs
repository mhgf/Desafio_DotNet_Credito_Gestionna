using System.Text.Json;
using Shared.Json.Converters;

namespace Shared.Json;

public static class CustomJsonSerializer
{
    private static readonly JsonSerializerOptions _settings = new JsonSerializerOptions
    {
        Converters = { new DateOnlyJsonConverter() }
    };

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _settings);
    }

    public static T? Deserialize<T>(string? json)
    {
        return JsonSerializer.Deserialize<T>(json, _settings);
    }
}