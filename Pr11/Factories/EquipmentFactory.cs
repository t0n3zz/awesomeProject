using Pr11.Models;


namespace Pr11.Factories
{
    public static class EquipmentFactory
    {
        private static Random random = new Random();

        public static Weapon CreateRandomWeapon()
        {
            return new Weapon
            {
                Attack = random.Next(5, 15)
            };
        }

        public static Armor CreateRandomArmor()
        {
            return new Armor
            {
                Defense = random.Next(3, 10)
            };
        }

        public static object CreateRandomEquipment()
        {
            if (random.Next(2) == 0)
            {
                return CreateRandomWeapon();
            }
            else
            {
                return CreateRandomArmor();
            }
        }
    }
}