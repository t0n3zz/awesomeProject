namespace Pr16.Models
{
    public class Player
    {
        public int MaxHP { get; } = 100;
        public int HP { get; private set; } = 100;

        public Weapon Weapon { get; private set; }
        public Armor Armor { get; private set; }

        public bool IsAlive => HP > 0;

        public int Attack => 5 + (Weapon?.Attack ?? 0);
        public int Defense => 3 + (Armor?.Defense ?? 0);

        public bool IsFrozen { get; set; }
        public bool IsDefending { get; private set; }

        public void EnterDefenseMode()
        {
            IsDefending = true;
        }

        public void ResetDefenseMode()
        {
            IsDefending = false;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                damage = 0;

            HP -= damage;

            if (HP < 0)
                HP = 0;
        }

        public void HealFull()
        {
            HP = MaxHP;
        }

        public void EquipWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }

        public void EquipArmor(Armor armor)
        {
            Armor = armor;
        }
    }
}
