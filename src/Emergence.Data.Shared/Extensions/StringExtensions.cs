using System.Text.RegularExpressions;
using Emergence.Data.Shared.Models;

namespace Emergence.Data.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return source;
            }
            else
            {
                return null;
            }
        }

        public static Zone ParseZone(this string zone)
        {
            if (!Regex.IsMatch(zone, "^([0-9]{1,2}[ab]?)$", RegexOptions.IgnoreCase))
            {
                return null;
            }

            string zoneLetter = null;
            if (Regex.IsMatch(zone, "[ab]", RegexOptions.IgnoreCase))
            {
                zoneLetter = Regex.Match(zone, "[ab]", RegexOptions.IgnoreCase).Value;
                zone = zone.Replace(zoneLetter, "");
            }
            
            int.TryParse(zone, out var zoneNumber);

            return new Zone
            {
                Number = zoneNumber,
                Letter = zoneLetter?.ToLowerInvariant()
            };
        }
    }
}
