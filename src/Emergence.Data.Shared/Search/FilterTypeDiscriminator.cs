using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Emergence.Data.Shared.Search
{
    public class FilterTypeDiscriminator<TValue> : JsonConverter<Filter> where TValue : class
    {
        public override bool CanConvert(Type typeToConvert) => typeof(Filter).IsAssignableFrom(typeToConvert);

        public override Filter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            var propertyName = reader.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            var typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            var filter = typeDiscriminator switch
            {
                TypeDiscriminator.Filter => new Filter<TValue>(),
                TypeDiscriminator.SelectFilter => new SelectFilter<TValue>(),
                TypeDiscriminator.RangeFilter => new RangeFilter<TValue>(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return filter;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Value":
                            var value = reader.GetString();
                            filter.Value = value as TValue;
                            break;
                        case "MinimumValue":
                            var minValue = reader.GetString();
                            ((RangeFilter<TValue>)filter).MinimumValue = minValue as TValue;
                            break;
                        case "MaximumValue":
                            var maxValue = reader.GetString();
                            ((RangeFilter<TValue>)filter).MinimumValue = maxValue as TValue;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Filter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is Filter<TValue> valueFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Filter);
                writer.WriteString("Value", valueFilter.Value as string);
            }
            else if (value is SelectFilter<TValue> selectFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.SelectFilter);
                writer.WriteString("Value", selectFilter.Value as string);
            }
            else if (value is RangeFilter<TValue> rangeFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.RangeFilter);
                writer.WriteString("MinimumValue", rangeFilter.MinimumValue as string);
                writer.WriteString("MaximumValue", rangeFilter.MaximumValue as string);
            }

            writer.WriteString("Name", value.Name);
            writer.WriteString("InputType", value.InputType.ToString());
            writer.WriteString("FilterType", value.FilterType.ToString());

            writer.WriteEndObject();
        }
    }

    public enum TypeDiscriminator
    {
        Filter = 0,
        SelectFilter = 1,
        RangeFilter = 2
    }
}
