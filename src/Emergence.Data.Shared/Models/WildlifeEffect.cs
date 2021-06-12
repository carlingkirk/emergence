using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared
{
    public class WildlifeEffect
    {
        [JsonConverter(typeof(EnumDisplayConverter<Wildlife>))]
        public Wildlife Wildlife { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Effect>))]
        public Effect Effect { get; set; }
    }
}
