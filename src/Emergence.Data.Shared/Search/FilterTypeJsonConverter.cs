using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Emergence.Data.Shared.Search
{
    public class FilterTypeJsonConverter<T> : JsonConverter<T>
        where T : class
    {
        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            if (reader.GetString() != ConverterInfo.TypeInfoName)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var converterInfo = ConverterInfo.GetConverterInfo(reader.GetString());
            var value = Activator.CreateInstance(converterInfo.Type) as T;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return value;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    var propertyInfo = converterInfo.Properties.FirstOrDefault(p => p.Name == propertyName);
                    propertyInfo.SetValue(value, JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options));
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var converterInfo = ConverterInfo.GetConverterInfo(value.GetType());
            // Poly writer
            writer.WriteString(ConverterInfo.TypeInfoName, converterInfo.TypeDiscriminator);

            foreach (var propertyInfo in converterInfo.Properties)
            {
                writer.WritePropertyName(propertyInfo.Name);
                JsonSerializer.Serialize(writer, propertyInfo.GetValue(value), propertyInfo.PropertyType, options);
            }
            writer.WriteEndObject();
        }
    }
}
