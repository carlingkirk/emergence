using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Emergence.Data.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string ToFriendlyName(this Enum source)
        {
            var enumType = source.GetType();
            var enumName = Enum.GetName(enumType, source);

            if (enumName != null)
            {
                var field = enumType.GetField(enumName);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return source.ToString();
        }

        public static string GetDisplayValue<T>(this string value) where T : struct, Enum
        {
            T enumValue;

            if (string.IsNullOrEmpty(value))
            {
                enumValue = Enum.Parse<T>("0");
            }
            else
            {
                enumValue = Enum.Parse<T>(value);
            }

            return enumValue.ToFriendlyName();
        }

        public static T GetEnumValue<T>(this string value) where T : struct, Enum
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var enumValue in enumValues)
            {
                if (value == enumValue.ToFriendlyName())
                {
                    return enumValue;
                }
            }
            return default;
        }

        public static Dictionary<string, long?> GetFacetValues<TEnum>(this Dictionary<string, long?> values, string defaultValue) where TEnum : struct, Enum
        {
            if (defaultValue != null && !values.Any(v => v.Key.ToString() == defaultValue.ToString()))
            {
                values = values.Prepend(new KeyValuePair<string, long?>(defaultValue, null)).ToDictionary(k => k.Key, v => v.Value);
            }

            values = values
                .Select(v => new KeyValuePair<string, long?>(v.Key.GetDisplayValue<TEnum>(), v.Value))
                .ToDictionary(k => k.Key, v => v.Value);

            return values;
        }
    }
}
