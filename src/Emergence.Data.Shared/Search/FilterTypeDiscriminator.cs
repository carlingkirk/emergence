using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    public class FilterTypeDiscriminator<TValue> : JsonConverter<Filter>
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
                            filter.Value = GetValue(reader);
                            break;
                        case "MinimumValue":
                            ((RangeFilter<TValue>)filter).MinimumValue = GetValue(reader);
                            break;
                        case "MaximumValue":
                            ((RangeFilter<TValue>)filter).MaximumValue = GetValue(reader);
                            break;
                        case "Name":
                            var name = reader.GetString();
                            filter.Name = name;
                            break;
                        case "InputType":
                            var inputType = reader.GetString();
                            filter.InputType = Enum.Parse<InputType>(inputType);
                            break;
                        case "FilterType":
                            var filterType = reader.GetString();
                            filter.FilterType = Enum.Parse<FilterType>(filterType);
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Filter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is RangeFilter<TValue> rangeFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.RangeFilter);
                switch (rangeFilter.MinimumValue)
                {
                    case string:
                        writer.WriteString("MinimumValue", rangeFilter.MinimumValue as string);
                        writer.WriteString("MaximumValue", rangeFilter.MaximumValue as string);
                        break;
                    case int:
                        var minIntValue = BoxingSafeConverter<TValue, int>.Instance.Convert(rangeFilter.MinimumValue);
                        writer.WriteNumber("MinimumValue", minIntValue);

                        var maxIntValue = BoxingSafeConverter<TValue, int>.Instance.Convert(rangeFilter.MaximumValue);
                        writer.WriteNumber("MaximumValue", maxIntValue);
                        break;
                    case double:
                        var minDoubleValue = BoxingSafeConverter<TValue, double>.Instance.Convert(rangeFilter.MinimumValue);
                        writer.WriteNumber("MinimumValue", minDoubleValue);

                        var maxDoubleValue = BoxingSafeConverter<TValue, double>.Instance.Convert(rangeFilter.MaximumValue);
                        writer.WriteNumber("MaximumValue", maxDoubleValue);
                        break;
                    default:
                        break;
                }
            }
            else if (value is SelectFilter<TValue> selectFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.SelectFilter);

                switch (selectFilter.Value)
                {
                    case string:
                        writer.WriteString("Value", selectFilter.Value as string);
                        break;
                    case int:
                        if (int.TryParse(selectFilter.Value as string, out var intValue))
                        {
                            writer.WriteNumber("Value", intValue);
                        }
                        break;
                    case double:
                        if (double.TryParse(selectFilter.Value as string, out var doubleValue))
                        {
                            writer.WriteNumber("Value", doubleValue);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (value is Filter<TValue> valueFilter)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Filter);
                writer.WriteString("Value", valueFilter.Value as string);
            }

            writer.WriteString("Name", value.Name);
            writer.WriteString("InputType", value.InputType.ToString());
            writer.WriteString("FilterType", value.FilterType.ToString());

            writer.WriteEndObject();
        }

        private TValue GetValue(Utf8JsonReader reader)
        {
            TValue value = default;
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out var doubleValue))
                {
                    value = BoxingSafeConverter<double, TValue>.Instance.Convert(doubleValue);
                }
                else if (reader.TryGetInt32(out var intValue))
                {
                    value = BoxingSafeConverter<int, TValue>.Instance.Convert(intValue);
                }
            }
            else
            {
                value = BoxingSafeConverter<string, TValue>.Instance.Convert(reader.GetString());
            }
            return value;
        }
    }

    public enum TypeDiscriminator
    {
        Filter = 0,
        SelectFilter = 1,
        RangeFilter = 2
    }
}
