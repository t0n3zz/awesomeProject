using System;

namespace Pr11.Services
{
    public static class RandomChoice
    {
        private static Random random = new Random();
        
        public static int GetRandom(int min, int max) => random.Next(min, max);
        
        public static bool CheckPercent(int percent) => random.Next(100) < percent;
        
        public static T GetRandomItem<T>(params T[] items) => items[random.Next(items.Length)];
    }
}