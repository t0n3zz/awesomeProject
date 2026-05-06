using Pr16.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr16.Factories
{
    internal class EnemyFactory
    {
        public static Enemy CreateRandomEnemy(Random random)
        {
            int type = random.Next(3);

            switch (type)
            {
                case 0:
                    return new Goblin();
                case 1:
                    return new Skeleton();
                case 2:
                    return new Mage();
                default:
                    return new Goblin();
            }
        }
    }
}
