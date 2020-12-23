using System;
using System.Buffers;
using System.Text.Json;

namespace Emergence.Data.Shared.Search
{
    public static class FilterSerializer
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        static FilterSerializer()
        {
            Options.Converters.Add(new FilterTypeJsonConverter<Filter>());
        }

        public static string Serialize<T>(T data) => JsonSerializer.Serialize(data, Options);

        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);

        public static ReadOnlyMemory<byte> SerializeUtf8<T>(T data)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using var wr = new Utf8JsonWriter(buffer);
            JsonSerializer.Serialize(wr, data, Options);
            return buffer.WrittenMemory;
        }

        public static T DeserializeUtf8<T>(ReadOnlySpan<byte> utf8Json) => JsonSerializer.Deserialize<T>(utf8Json, Options);
    }
}
