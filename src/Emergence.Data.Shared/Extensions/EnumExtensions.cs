using System;
using System.ComponentModel;

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
            T stageValue;

            if (string.IsNullOrEmpty(value))
            {
                stageValue = Enum.Parse<T>("0");
            }
            else
            {
                stageValue = Enum.Parse<T>(value);

            }

            return stageValue.ToFriendlyName();
        }
    }
}
