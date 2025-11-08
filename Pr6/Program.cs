using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}

class Game
{
    private Player player;
    private Random random = new Random();
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
                if (random.Next(2) == 0)
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
        Enemy boss = GetRandomBoss();
        Fight(boss);
    }

    private void FightEnemy()
    {
        Enemy enemy = GetRandomEnemy();
        Console.WriteLine($"Появился {enemy.Name}!");
        Fight(enemy);
    }

    private void Fight(Enemy enemy)
    {
        while (player.IsAlive && enemy.IsAlive)
        {
            Console.WriteLine("1 - Атака, 2 - Защита");
            string input = Console.ReadLine();
            
            if (input == "2")
            {
                if (random.Next(100) < 40)
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

    private void OpenChest()
    {
        Console.WriteLine("Найден сундук!");
        int itemType = random.Next(3);
        
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

    private Enemy GetRandomEnemy()
    {
        int type = random.Next(3);
        return type switch
        {
            0 => new Goblin(),
            1 => new Skeleton(),
            2 => new Mage(),
            _ => new Goblin()
        };
    }

    private Enemy GetRandomBoss()
    {
        int type = random.Next(4);
        return type switch
        {
            0 => new VVG(),
            1 => new Kovalsky(),
            2 => new Archmage(),
            3 => new Pestov(),
            _ => new VVG()
        };
    }

    private Weapon GenerateWeapon() => new Weapon { Attack = random.Next(5, 15) };
    private Armor GenerateArmor() => new Armor { Defense = random.Next(3, 10) };
}

class Player
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
        Console.WriteLine($"Игрок получил {damage} урона. Осталось {HP} HP");
    }

    public void Heal() => HP = 100;

    public void EquipWeapon(Weapon newWeapon) => weapon = newWeapon;
    public void EquipArmor(Armor newArmor) => armor = newArmor;
}

abstract class Enemy
{
    public string Name { get; protected set; }
    public int HP { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }
    public bool IsAlive => HP > 0;

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Console.WriteLine($"{Name} получил {damage} урона");
    }

    public abstract void AttackPlayer(Player player);
}

class Goblin : Enemy
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

class Skeleton : Enemy
{
    public Skeleton()
    {
        Name = "Скелет";
        HP = 25;
        Attack = 7;
        Defense = 2;
    }

    public override void AttackPlayer(Player player)
    {
        player.TakeDamage(Attack);
    }
}

class Mage : Enemy
{
    public Mage()
    {
        Name = "Маг";
        HP = 20;
        Attack = 10;
        Defense = 1;
    }

    public override void AttackPlayer(Player player)
    {
        player.TakeDamage(Math.Max(0, Attack - player.Defense));
    }
}

class VVG : Goblin
{
    public VVG()
    {
        Name = "ВВГ";
        HP = (int)(HP * 2.0);
        Attack = (int)(Attack * 1.5);
        Defense = (int)(Defense * 1.2);
    }
}

class Kovalsky : Skeleton
{
    public Kovalsky()
    {
        Name = "Ковальский";
        HP = (int)(HP * 2.5);
        Attack = (int)(Attack * 1.3);
        Defense = (int)(Defense * 1.4);
    }
}

class Archmage : Mage
{
    public Archmage()
    {
        Name = "Архимаг C++";
        HP = (int)(HP * 1.8);
        Attack = (int)(Attack * 1.6);
        Defense = (int)(Defense * 1.1);
    }
}

class Pestov : Skeleton
{
    public Pestov()
    {
        Name = "Пестов С--";
        HP = (int)(HP * 1.3);
        Attack = (int)(Attack * 1.8);
        Defense = (int)(Defense * 0.6);
    }
}

class Weapon
{
    public int Attack { get; set; }
}

class Armor
{
    public int Defense { get; set; }
}