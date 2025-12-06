using System;
using Pr11.Models;
using Pr11.Models.Bosses;

namespace Pr11.Factories
{
    public static class EnemyFactory
    {
        private static Random random = new Random();
        
        public static Enemy CreateRandomEnemy()
        {
            int type = random.Next(4);
            
            return type switch
            {
                0 => new Goblin(),
                1 => new Skeleton(),
                2 => new Mage(),
                3 => new Slime(),
                _ => new Goblin()
            };
        }
        
        public static Enemy CreateRandomBoss()
        {
            int type = random.Next(4);
            
            return type switch
            {
                0 => new VVG(),
                1 => new Kovalsky(),
                2 => new Archmage(),
                3 => new Pestov(),
                _ => new VVG()
            };
        }
    }
}