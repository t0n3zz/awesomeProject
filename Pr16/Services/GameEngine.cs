using Pr16.Factories;
using Pr16.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pr16.Services
{
    public class GameEngine
    {
        private readonly Random _random = new Random();

        public Player Player { get; private set; }
        public int Turn { get; private set; }
        public GameState State { get; private set; }

        public List<Enemy> CurrentEnemies { get; private set; } = new List<Enemy>();
        public List<string> Log { get; } = new List<string>();

        public Item PendingItem { get; private set; }

        public void StartGame()
        {
            Player = new Player();
            Turn = 0;
            State = GameState.Exploration;
            CurrentEnemies.Clear();
            Log.Clear();
            PendingItem = null;

            AddLog("Игра началась.");
        }

        public void NextTurn()
        {
            if (State != GameState.Exploration || !Player.IsAlive)
                return;

            Turn++;

            if (Turn % 10 == 0)
            {
                SpawnBoss();
                return;
            }

            int roll = _random.Next(2);

            if (roll == 0)
                SpawnEnemies();
            else
                OpenChest();
        }

        private void SpawnEnemies()
        {
            CurrentEnemies.Clear();

            int count = _random.Next(1, 4);

            for (int i = 0; i < count; i++)
            {
                CurrentEnemies.Add(EnemyFactory.CreateRandomEnemy(_random));
            }

            State = GameState.Battle;
            AddLog("На этаже появились враги: " + string.Join(", ", CurrentEnemies.Select(e => e.Name)) + ".");
        }

        private void SpawnBoss()
        {
            CurrentEnemies.Clear();
            CurrentEnemies.Add(BossFactory.CreateRandomBoss(_random));

            State = GameState.Battle;
            AddLog("Появился босс: " + CurrentEnemies[0].Name + ".");
        }

        private void OpenChest()
        {
            PendingItem = ItemFactory.CreateRandomItem(_random);

            if (PendingItem is Potion)
            {
                Player.HealFull();
                AddLog("Найден " + PendingItem.Name + ". Игрок полностью восстановил здоровье.");
                State = GameState.Exploration;
                PendingItem = null;
                return;
            }

            State = GameState.LootChoice;
            AddLog("Найден предмет: " + PendingItem + ".");
        }

        public void TakeItem()
        {
            if (State != GameState.LootChoice || PendingItem == null)
                return;

            Weapon weapon = PendingItem as Weapon;
            Armor armor = PendingItem as Armor;

            if (weapon != null)
            {
                Player.EquipWeapon(weapon);
                AddLog("Игрок взял оружие: " + weapon + ".");
            }
            else if (armor != null)
            {
                Player.EquipArmor(armor);
                AddLog("Игрок взял доспех: " + armor + ".");
            }

            PendingItem = null;
            State = GameState.Exploration;
        }

        public void DiscardItem()
        {
            if (State != GameState.LootChoice || PendingItem == null)
                return;

            AddLog("Игрок выбросил предмет: " + PendingItem + ".");
            PendingItem = null;
            State = GameState.Exploration;
        }

        public void PlayerAttack(int enemyIndex)
        {
            if (State != GameState.Battle)
                return;

            if (Player.IsFrozen)
            {
                Player.IsFrozen = false;
                AddLog("Игрок пропускает ход из-за заморозки.");
                EnemiesTurn();
                return;
            }

            if (enemyIndex < 0 || enemyIndex >= CurrentEnemies.Count)
                return;

            Enemy enemy = CurrentEnemies[enemyIndex];

            int damage = Math.Max(0, Player.Attack - enemy.Defense);
            enemy.TakeDamage(damage);

            AddLog("Игрок атаковал " + enemy.Name + " и нанёс " + damage + " урона.");

            if (!enemy.IsAlive)
            {
                AddLog(enemy.Name + " побеждён.");
                CurrentEnemies.RemoveAt(enemyIndex);
            }

            if (CurrentEnemies.Count == 0)
            {
                State = GameState.Exploration;
                AddLog("Бой завершён.");
                return;
            }

            EnemiesTurn();
        }

        public void PlayerDefend()
        {
            if (State != GameState.Battle)
                return;

            if (Player.IsFrozen)
            {
                Player.IsFrozen = false;
                AddLog("Игрок пропускает ход из-за заморозки.");
                EnemiesTurn();
                return;
            }

            Player.EnterDefenseMode();
            AddLog("Игрок занял защитную стойку.");

            EnemiesTurn();
        }

        private void EnemiesTurn()
        {
            foreach (Enemy enemy in CurrentEnemies.ToList())
            {
                if (!Player.IsAlive)
                    break;

                int damage = enemy.Attack;
                bool evaded = false;

                if (Player.IsDefending)
                {
                    if (_random.Next(100) < 40)
                    {
                        evaded = true;
                        AddLog("Игрок уклонился от атаки врага " + enemy.Name + ".");
                    }
                    else
                    {
                        int blockPercent = _random.Next(70, 101);
                        int blockedAmount = Player.Defense * blockPercent / 100;
                        damage -= blockedAmount;
                        AddLog("Игрок заблокировал " + blockedAmount + " урона.");
                    }
                }
                else if (!enemy.IgnoresArmor)
                {
                    damage -= Player.Defense;
                }

                if (evaded)
                    continue;

                if (enemy.CritChance > 0 && _random.Next(100) < enemy.CritChance)
                {
                    damage *= 2;
                    AddLog(enemy.Name + " наносит критический удар!");
                }

                damage = Math.Max(0, damage);
                Player.TakeDamage(damage);

                AddLog(enemy.Name + " атакует игрока и наносит " + damage + " урона.");

                if (enemy.FreezeChance > 0 && _random.Next(100) < enemy.FreezeChance)
                {
                    Player.IsFrozen = true;
                    AddLog("Игрок заморожен и пропустит следующий ход.");
                }
            }

            Player.ResetDefenseMode();

            if (!Player.IsAlive)
            {
                State = GameState.GameOver;
                AddLog("Игрок погиб.");
            }
        }

        private void AddLog(string message)
        {
            Log.Add(message);
        }
    }
}
