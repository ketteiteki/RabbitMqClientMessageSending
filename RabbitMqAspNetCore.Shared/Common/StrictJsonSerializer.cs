using System.Text;
using System.Text.Json;

namespace RabbitMqAspNetCore.Shared.Common;

public static class StrictJsonSerializer
{
    // private static readonly JsonSerializerOptions JsonSerializerOptions = new();
    
    public static bool TryDeserialize(string value, Type type, out object result)
    {
        try
        {
            var converter = new StrictJsonConverterFactory().CreateConverter(type, null);

            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(converter);
            
            result = JsonSerializer.Deserialize(Encoding.UTF8.GetBytes(value), type, jsonSerializerOptions);
            return true;
        }
        catch(Exception e)
        {
            result = default;
            return false;
        }
    }
}