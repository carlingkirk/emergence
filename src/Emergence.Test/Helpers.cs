using System;

namespace Emergence.Test
{
    public static class Helpers
    {
        public static DateTime Today => DateTime.UtcNow.AddDays(GetRandom() * -1);
        public static int GetRandom(int max = 100)
        {
            var random = new Random();
            return random.Next(1, max);
        }

        public static string UserId => "39E22869-A6AB-496E-B490-731FF49EF33B";
        public static string PrivateUserId => "2876E16E-F579-48C3-8BCA-F64C11590879";
    }
}
