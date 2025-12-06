using System;

namespace Pr11.Models
{
    public class Slime : Enemy
    {
        public Slime()
        {
            Name = "Слизень";
            HP = 35;
            Attack = 6;
            Defense = 1;
        }
        
        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(0, damage - 2);
            HP -= reducedDamage;
            Console.WriteLine($"{Name} поглотил часть урона и получил {reducedDamage} вместо {damage}");
        }
        
        public override void AttackPlayer(Player player)
        {
            int damage = Attack;
            if (new Random().Next(100) < 20)
            {
                damage += 3;
                Console.WriteLine($"{Name} отравил вас!");
            }
            player.TakeDamage(Math.Max(0, damage - player.Defense));
        }
    }
}