using System;
using Pr11.Models;
using Pr11.Factories;
using Pr11.Services;

namespace Pr11.Services
{
    public class GameService
    {
        private Player player;
        private int turnCount = 0;

        public void Start()
        {
            player = new Player();

            while (player.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\nХод {turnCount}");
                
                if (turnCount % 10 == 0)
                {
                    FightBoss();
                }
                else
                {
                    if (RandomChoice.CheckPercent(50))
                    {
                        FightEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }
            }
        }

        private void FightBoss()
        {
            Console.WriteLine("Появился босс!");
            Enemy boss = EnemyFactory.CreateRandomBoss();
            Fight(boss);
        }

        private void FightEnemy()
        {
            Enemy enemy = EnemyFactory.CreateRandomEnemy();
            Console.WriteLine($"Появился {enemy.Name}!");
            Fight(enemy);
        }

        private void Fight(Enemy enemy)
        {
            var combatService = new CombatService();
            combatService.Fight(player, enemy);
        }

        private void OpenChest()
        {
            Console.WriteLine("Найден сундук!");
            int itemType = RandomChoice.GetRandom(0, 3);
            
            switch (itemType)
            {
                case 0:
                    Console.WriteLine("Зелье здоровья!");
                    player.Heal();
                    break;
                case 1:
                    Weapon newWeapon = GenerateWeapon();
                    Console.WriteLine($"Найдено оружие: Атака {newWeapon.Attack}");
                    Console.WriteLine("1 - Взять, 2 - Выбросить");
                    if (Console.ReadLine() == "1")
                    {
                        player.EquipWeapon(newWeapon);
                    }
                    break;
                case 2:
                    Armor newArmor = GenerateArmor();
                    Console.WriteLine($"Найдены доспехи: Защита {newArmor.Defense}");
                    Console.WriteLine("1 - Взять, 2 - Выбросить");
                    if (Console.ReadLine() == "1")
                    {
                        player.EquipArmor(newArmor);
                    }
                    break;
            }
        }

        private Weapon GenerateWeapon() => new Weapon { Attack = RandomChoice.GetRandom(5, 15) };
        private Armor GenerateArmor() => new Armor { Defense = RandomChoice.GetRandom(3, 10) };
    }
}