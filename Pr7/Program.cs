using System;
using System.Collections.Generic;
using System.Linq;

namespace CarServiceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Player player;
        private Warehouse warehouse;
        private List<CarPart> availableParts;
        private Random random = new Random();
        private int customerCount = 0;
        private Queue<PurchaseOrder> purchaseOrders = new Queue<PurchaseOrder>();

        public void Start()
        {
            InitializeGame();
            
            while (player.Money > 0)
            {
                Console.Clear();
                Console.WriteLine($"Баланс: {player.Money}₽ | Клиентов обслужено: {customerCount}");
                Console.WriteLine("1 - Новый клиент");
                Console.WriteLine("2 - Склад");
                Console.WriteLine("3 - Закупки");
                Console.WriteLine("4 - Выход");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": HandleCustomer(); break;
                    case "2": ShowWarehouse(); break;
                    case "3": ShowPurchaseMenu(); break;
                    case "4": return;
                }
            }
            
            Console.WriteLine("Вы банкрот! Игра окончена.");
        }

        private void InitializeGame()
        {
            player = new Player { Money = 1000 };
            warehouse = new Warehouse();
            availableParts = new List<CarPart>
            {
                new CarPart { Name = "Тормозные колодки", Price = 50 },
                new CarPart { Name = "Масляный фильтр", Price = 30 },
                new CarPart { Name = "Воздушный фильтр", Price = 25 },
                new CarPart { Name = "Свечи зажигания", Price = 40 },
                new CarPart { Name = "Аккумулятор", Price = 100 },
                new CarPart { Name = "Шины", Price = 80 },
                new CarPart { Name = "Тормозная жидкость", Price = 20 },
                new CarPart { Name = "Лобовое стекло", Price = 150 }
            };

            warehouse.AddPart(availableParts[0], 3);
            warehouse.AddPart(availableParts[1], 2);
        }

        private void HandleCustomer()
        {
            customerCount++;
            ProcessPurchaseOrders();

            CarPart brokenPart = availableParts[random.Next(availableParts.Count)];
            int repairCost = brokenPart.Price + 50;
            
            Console.WriteLine($"Клиент #{customerCount}");
            Console.WriteLine($"Сломано: {brokenPart.Name}");
            Console.WriteLine($"Стоимость ремонта: {repairCost}₽");
            Console.WriteLine("1 - Взять заказ");
            Console.WriteLine("2 - Отказаться");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                if (warehouse.HasPart(brokenPart))
                {
                    warehouse.RemovePart(brokenPart, 1);
                    player.Money += repairCost;
                    Console.WriteLine($"Ремонт выполнен! Получено {repairCost}₽");
                }
                else
                {
                    Console.WriteLine("Детали нет на складе! Клиент недоволен.");
                    player.Money -= repairCost * 2;
                    Console.WriteLine($"Штраф: {repairCost * 2}₽");
                }
            }
            else
            {
                int fine = 20;
                player.Money -= fine;
                Console.WriteLine($"Штраф за отказ: {fine}₽");
            }
            
            Console.ReadKey();
        }

        private void ShowWarehouse()
        {
            Console.Clear();
            Console.WriteLine("=== СКЛАД ===");
            foreach (var item in warehouse.GetAllItems())
            {
                Console.WriteLine($"{item.Part.Name}: {item.Quantity} шт.");
            }
            Console.ReadKey();
        }

        private void ShowPurchaseMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАКУПКИ ===");
            
            for (int i = 0; i < availableParts.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {availableParts[i].Name} ({availableParts[i].Price}₽)");
            }
            
            Console.WriteLine("0 - Назад");
            Console.Write("Выберите деталь: ");
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= availableParts.Count)
            {
                Console.Write("Количество: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    CarPart part = availableParts[choice - 1];
                    int totalCost = part.Price * quantity;
                    
                    if (player.Money >= totalCost)
                    {
                        player.Money -= totalCost;
                        purchaseOrders.Enqueue(new PurchaseOrder { Part = part, Quantity = quantity, ArrivalTime = customerCount + 2 });
                        Console.WriteLine($"Заказ оформлен! Прибудет через 2 клиента.");
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно денег!");
                    }
                }
            }
            
            Console.ReadKey();
        }

        private void ProcessPurchaseOrders()
        {
            while (purchaseOrders.Count > 0 && purchaseOrders.Peek().ArrivalTime <= customerCount)
            {
                PurchaseOrder order = purchaseOrders.Dequeue();
                warehouse.AddPart(order.Part, order.Quantity);
                Console.WriteLine($"Поставка прибыла: {order.Part.Name} x{order.Quantity}");
            }
        }
    }

    class Player
    {
        public int Money { get; set; }
    }

    class CarPart
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    class Warehouse
    {
        private List<WarehouseItem> items = new List<WarehouseItem>();

        public void AddPart(CarPart part, int quantity)
        {
            var existing = items.FirstOrDefault(i => i.Part.Name == part.Name);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                items.Add(new WarehouseItem { Part = part, Quantity = quantity });
            }
        }

        public bool HasPart(CarPart part)
        {
            return items.Any(i => i.Part.Name == part.Name && i.Quantity > 0);
        }

        public void RemovePart(CarPart part, int quantity)
        {
            var item = items.FirstOrDefault(i => i.Part.Name == part.Name);
            if (item != null)
            {
                item.Quantity -= quantity;
            }
        }

        public List<WarehouseItem> GetAllItems()
        {
            return items.Where(i => i.Quantity > 0).ToList();
        }
    }

    class WarehouseItem
    {
        public CarPart Part { get; set; }
        public int Quantity { get; set; }
    }

    class PurchaseOrder
    {
        public CarPart Part { get; set; }
        public int Quantity { get; set; }
        public int ArrivalTime { get; set; }
    }
}