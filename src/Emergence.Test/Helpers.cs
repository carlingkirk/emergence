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
    }
}
