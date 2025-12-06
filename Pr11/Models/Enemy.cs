using System;

namespace Pr11.Models
{
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int HP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        public bool IsAlive => HP > 0;

        public virtual void TakeDamage(int damage)
        {
            HP -= damage;
            Console.WriteLine($"{Name} получил {damage} урона");
        }

        public abstract void AttackPlayer(Player player);
    }
}