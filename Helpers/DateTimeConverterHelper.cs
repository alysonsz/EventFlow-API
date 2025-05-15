using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventFlow_API.Helpers;

public class DateTimeConverterHelper : JsonConverter<DateTime>
{
    private const string DateFormat = "dd/MM/yyyy";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        if (DateTime.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return date;

        if (DateTime.TryParse(value, out date))
            return date;

        throw new JsonException($"Data no formato inválido: {value}. Esperado ISO ou {DateFormat}.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}

