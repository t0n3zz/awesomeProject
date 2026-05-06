using Pr16.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr16.Factories
{
    internal class ItemFactory
    {
        public static Item CreateRandomItem(Random random)
        {
            int type = random.Next(3);

            switch (type)
            {
                case 0:
                    return new Potion
                    {
                        Name = "Эликсир жизни"
                    };
                case 1:
                    return new Weapon
                    {
                        Name = "Меч",
                        Attack = random.Next(5, 16)
                    };
                case 2:
                    return new Armor
                    {
                        Name = "Броня",
                        Defense = random.Next(3, 11)
                    };
                default:
                    return new Potion
                    {
                        Name = "Эликсир жизни"
                    };
            }
        }
    }
}
