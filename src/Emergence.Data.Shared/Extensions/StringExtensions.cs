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
    }
}
