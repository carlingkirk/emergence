using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Emergence.Data.Shared
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Wildlife
    {
        Bees,
        Beetles,
        Birds,
        Butterflies,
        Hummingbirds,
        Moths,
        Wasps
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Effect
    {
        [Description("")]
        Unknown,
        Food,
        Host
    }
}
