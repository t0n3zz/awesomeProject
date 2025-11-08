using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace
{
    class Program
    {
        static void Main(string[] args)
        {
            MarketplaceApp app = new MarketplaceApp();
            app.Run();
        }
    }

    class MarketplaceApp
    {
        private Database db;
        private User currentUser;
        
        public MarketplaceApp()
        {
            db = new Database();
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GMWOG||GG.MOW||WONGG ===");
                
                if (currentUser == null)
                {
                    ShowMainMenu();
                }
                else
                {
                    ShowUserMenu();
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("1 - Регистрация");
            Console.WriteLine("2 - Вход");
            Console.WriteLine("3 - Просмотр товаров");
            Console.WriteLine("4 - Выход");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1": Register(); break;
                case "2": Login(); break;
                case "3": ShowProducts(); break;
                case "4": Environment.Exit(0); break;
            }
        }

        private void ShowUserMenu()
        {
            Console.WriteLine($"Добро пожаловать, {currentUser.Username}!");
            Console.WriteLine("1 - Товары");
            Console.WriteLine("2 - Корзина");
            Console.WriteLine("3 - Мои заказы");
            Console.WriteLine("4 - Выйти");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1": ShowProducts(); break;
                case "2": ShowCart(); break;
                case "3": ShowOrders(); break;
                case "4": currentUser = null; break;
            }
        }

        private void Register()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ ===");
            
            User newUser = new User();
            
            Console.Write("Логин: ");
            newUser.Username = Console.ReadLine();
            
            Console.Write("Пароль: ");
            string password1 = Console.ReadLine();
            
            Console.Write("Повторите пароль: ");
            string password2 = Console.ReadLine();
            
            if (password1 != password2)
            {
                Console.WriteLine("Пароли не совпадают!");
                Console.ReadKey();
                return;
            }
            
            newUser.Password = password1;
            
            if (db.Users.Any(u => u.Username == newUser.Username))
            {
                Console.WriteLine("Логин занят!");
                Console.ReadKey();
                return;
            }
            
            db.Users.Add(newUser);
            currentUser = newUser;
            Console.WriteLine("Регистрация успешна!");
            Console.ReadKey();
        }

        private void Login()
        {
            Console.Clear();
            Console.WriteLine("=== ВХОД ===");
            
            Console.Write("Логин: ");
            string username = Console.ReadLine();
            
            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            
            User user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine("Вход выполнен!");
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль!");
            }
            
            Console.ReadKey();
        }

        private void ShowProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ТОВАРЫ ===");
            
            foreach (var product in db.Products)
            {
                Console.WriteLine($"{product.Id}. {product.Name} - {product.Price}₽");
                Console.WriteLine($"   {product.Description}");
            }
            
            if (currentUser != null)
            {
                Console.WriteLine("\nВведите ID товара для добавления в корзину (0 - назад):");
                if (int.TryParse(Console.ReadLine(), out int productId) && productId > 0)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        AddToCart(product);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nДля покупок необходимо войти в аккаунт");
                Console.ReadKey();
            }
        }

        private void AddToCart(Product product)
        {
            var cartItem = currentUser.Cart.FirstOrDefault(i => i.Product.Id == product.Id);
            
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                currentUser.Cart.Add(new CartItem { Product = product, Quantity = 1 });
            }
            
            Console.WriteLine("Товар добавлен в корзину!");
            Console.ReadKey();
        }
        private void ShowCart()
        {
            Console.Clear();
            Console.WriteLine("=== КОРЗИНА ===");
            
            if (!currentUser.Cart.Any())
            {
                Console.WriteLine("Корзина пуста");
                Console.ReadKey();
                return;
            }
            
            decimal total = 0;
            foreach (var item in currentUser.Cart)
            {
                decimal itemTotal = item.Product.Price * item.Quantity;
                total += itemTotal;
                Console.WriteLine($"{item.Product.Id}. {item.Product.Name} x{item.Quantity} = {itemTotal}₽");
            }
            Console.WriteLine($"Общая сумма: {total}₽");
            
            Console.WriteLine("\n1 - Купить все");
            Console.WriteLine("2 - Купить один товар");
            Console.WriteLine("3 - Удалить товар");
            Console.WriteLine("0 - Назад");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1": CheckoutAll(); break;
                case "2": CheckoutSingle(); break;
                case "3": RemoveFromCart(); break;
            }
        }

        private void CheckoutAll()
        {
            Console.WriteLine("Выберите ПВЗ:");
            foreach (var pickup in db.PickupPoints)
            {
                Console.WriteLine($"{pickup.Id}. {pickup.Address}");
            }
            
            if (int.TryParse(Console.ReadLine(), out int pickupId))
            {
                var pickup = db.PickupPoints.FirstOrDefault(p => p.Id == pickupId);
                if (pickup != null)
                {
                    var order = new Order
                    {
                        User = currentUser,
                        PickupPoint = pickup,
                        OrderDate = DateTime.Now,
                        Items = new List<OrderItem>()
                    };
                    
                    foreach (var cartItem in currentUser.Cart)
                    {
                        order.Items.Add(new OrderItem 
                        { 
                            Product = cartItem.Product, 
                            Quantity = cartItem.Quantity 
                        });
                    }
                    
                    db.Orders.Add(order);
                    currentUser.Cart.Clear();
                    
                    Console.WriteLine("Заказ оформлен!");
                    Console.ReadKey();
                }
            }
        }

        private void CheckoutSingle()
        {
            Console.Write("Введите ID товара для покупки: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var cartItem = currentUser.Cart.FirstOrDefault(i => i.Product.Id == productId);
                if (cartItem != null)
                {
                    Console.WriteLine("Выберите ПВЗ:");
                    foreach (var pickup in db.PickupPoints)
                    {
                        Console.WriteLine($"{pickup.Id}. {pickup.Address}");
                    }
                    
                    if (int.TryParse(Console.ReadLine(), out int pickupId))
                    {
                        var pickup = db.PickupPoints.FirstOrDefault(p => p.Id == pickupId);
                        if (pickup != null)
                        {
                            var order = new Order
                            {
                                User = currentUser,
                                PickupPoint = pickup,
                                OrderDate = DateTime.Now,
                                Items = new List<OrderItem>
                                {
                                    new OrderItem { Product = cartItem.Product, Quantity = cartItem.Quantity }
                                }
                            };
                            
                            db.Orders.Add(order);
                            currentUser.Cart.Remove(cartItem);
                            
                            Console.WriteLine("Заказ оформлен!");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

        private void RemoveFromCart()
        {
            Console.Write("Введите ID товара для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var cartItem = currentUser.Cart.FirstOrDefault(i => i.Product.Id == productId);
                if (cartItem != null)
                {
                    currentUser.Cart.Remove(cartItem);
                    Console.WriteLine("Товар удален из корзины!");
                    Console.ReadKey();
                }
            }
        }

        private void ShowOrders()
        {
            Console.Clear();
            Console.WriteLine("=== МОИ ЗАКАЗЫ ===");
            
            var userOrders = db.Orders
                .Where(o => o.User.Id == currentUser.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            
            if (!userOrders.Any())
            {
                Console.WriteLine("Заказов нет");
                Console.ReadKey();
                return;
            }
            
            foreach (var order in userOrders)
            {
                Console.WriteLine($"Заказ от {order.OrderDate}");
                Console.WriteLine($"ПВЗ: {order.PickupPoint.Address}");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  {item.Product.Name} x{item.Quantity} = {item.Product.Price * item.Quantity}₽");
                }
                Console.WriteLine();
            }
            
            Console.ReadKey();
        }
    }

    class Database
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<PickupPoint> PickupPoints { get; set; } = new List<PickupPoint>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public Database()
        {
            Products.AddRange(new[]
            {
                new Product { Id = 1, Name = "iPhone 17", Price = 99999, Description = "Новый айфон" },
                new Product { Id = 2, Name = "MacBook M5 Pro", Price = 199999, Description = "Ноутбук для работы" },
                new Product { Id = 3, Name = "AirPods 3", Price = 19999, Description = "Беспроводные наушники" }
            });

            PickupPoints.AddRange(new[]
            {
                new PickupPoint { Id = 1, Address = "ул. Пушкина, д. 1" },
                new PickupPoint { Id = 2, Address = "пр. Ленина, д. 25" },
                new PickupPoint { Id = 3, Address = "ш. Московское, д. 10" }
            });
        }
    }

    class User
    {
        private static int nextId = 1;
        public int Id { get; set; } = nextId++;
        public string Username { get; set; }
        public string Password { get; set; }
        public List<CartItem> Cart { get; set; } = new List<CartItem>();
    }

    class Product
    {
        private static int nextId = 1;
        public int Id { get; set; } = nextId++;
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    class PickupPoint
    {
        private static int nextId = 1;
        public int Id { get; set; } = nextId++;
        public string Address { get; set; }
    }

    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    class Order
    {
        private static int nextId = 1;
        public int Id { get; set; } = nextId++;
        public User User { get; set; }
        public PickupPoint PickupPoint { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}