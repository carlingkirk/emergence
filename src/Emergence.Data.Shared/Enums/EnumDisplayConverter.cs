using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Enums
{
    public class EnumDisplayConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var enumValue in enumValues)
            {
                if (enumString == enumValue.ToFriendlyName())
                {
                    return enumValue;
                }
            }

            return Enum.Parse<T>("");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToFriendlyName());
    }
}
