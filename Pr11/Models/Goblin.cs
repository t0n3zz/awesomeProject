using System;

namespace Pr11.Models
{
    public class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            HP = 30;
            Attack = 8;
            Defense = 3;
        }

        public override void AttackPlayer(Player player)
        {
            int damage = Attack;
            if (new Random().Next(100) < 15)
            {
                damage *= 2;
                Console.WriteLine("Критический удар!");
            }
            player.TakeDamage(Math.Max(0, damage - player.Defense));
        }
    }
}