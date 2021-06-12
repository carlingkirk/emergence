using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared
{
    [JsonConverter(typeof(EnumDisplayConverter<SoilType>))]
    public enum SoilType
    {
        Fertile,
        Loamy,
        Rocky,
        Clay,
        Peaty,
        Swamp,
        Water
    }
}
