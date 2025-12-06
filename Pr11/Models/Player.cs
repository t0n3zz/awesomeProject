namespace Pr11.Models
{
    public class Player
    {
        public int HP { get; private set; } = 100;
        public int Attack => weapon?.Attack ?? 5;
        public int Defense => armor?.Defense ?? 2;
        public bool IsAlive => HP > 0;

        private Weapon weapon;
        private Armor armor;

        public void TakeDamage(int damage)
        {
            HP -= damage;
            Console.WriteLine($"Игрок получил {damage} урона и теперь у него осталось {HP} HP");
        }

        public void Heal() => HP = 100;

        public void EquipWeapon(Weapon newWeapon) => weapon = newWeapon;
        public void EquipArmor(Armor newArmor) => armor = newArmor;
    }
}