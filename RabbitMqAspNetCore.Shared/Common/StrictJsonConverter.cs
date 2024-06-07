using System.Text.Json;
using System.Text.Json.Serialization;

namespace RabbitMqAspNetCore.Shared.Common;

public class StrictJsonConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var properties = typeof(T).GetProperties();
        var propertyNamesOfType = properties.Select(x => x.Name).ToArray();
        var propertyNamesOfJson = jsonDocument.RootElement.EnumerateObject().Select(x => x.Name).ToArray();

        var missingInJson = propertyNamesOfType.Except(propertyNamesOfJson);
        var missingInType = propertyNamesOfJson.Except(propertyNamesOfType);

        if (missingInJson.Any() || missingInType.Any())
        {
            throw new JsonException("Unknown property");
        }

        var json = jsonDocument.RootElement.GetRawText();
        return JsonSerializer.Deserialize<T>(json);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
