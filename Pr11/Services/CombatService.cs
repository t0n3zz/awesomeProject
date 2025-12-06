using System;
using Pr11.Models;

namespace Pr11.Services
{
    public class CombatService
    {
        private Random random = new Random();
        
        public void Fight(Player player, Enemy enemy)
        {
            while (player.IsAlive && enemy.IsAlive)
            {
                Console.WriteLine("1 - Атака, 2 - Защита");
                string input = Console.ReadLine();
                
                if (input == "2")
                {
                    if (RandomChoice.CheckPercent(40))
                    {
                        Console.WriteLine("Уклонение!");
                        continue;
                    }
                }

                int damage = player.Attack - enemy.Defense;
                if (damage > 0)
                {
                    enemy.TakeDamage(damage);
                    Console.WriteLine($"Вы нанесли {damage} урона");
                }

                if (!enemy.IsAlive) break;

                enemy.AttackPlayer(player);
            }

            if (player.IsAlive)
            {
                Console.WriteLine("Враг побежден!");
            }
        }
    }
}