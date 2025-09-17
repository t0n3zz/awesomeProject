using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem
{
    public enum ProductCategory
    {
        Food, Beverages, Stationery, Electronics
    }

    public struct InventoryItem
    {
        private static List<InventoryItem> _inventory = new List<InventoryItem>();
        private static Stack<SaleTransaction> _salesHistory = new Stack<SaleTransaction>();
        
        public uint Id { get; private set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public uint StockQuantity { get; set; }
        public bool IsAvailableInWarehouse { get; set; }
        public ProductCategory Category { get; set; }

        public InventoryItem(string name, decimal price, uint quantity, 
                           bool warehouseAvailable, ProductCategory category)
        {
            Name = name;
            UnitPrice = price;
            StockQuantity = quantity;
            IsAvailableInWarehouse = warehouseAvailable;
            Category = category;
            Id = (uint)_inventory.Count + 1;
        }

        public void RegisterItem()
        {
            _inventory.Add(this);
        }

        public void ModifyItem()
        {
            RemoveFromInventory();
            _inventory.Add(this);
        }

        public void RemoveFromInventory()
        {
            _inventory.RemoveAll(item => item.Id == Id);
        }

        public bool Restock(uint additionalQuantity)
        {
            if (!IsAvailableInWarehouse) return false;
            
            StockQuantity += additionalQuantity;
            ModifyItem();
            return true;
        }

        public bool ProcessSale(uint saleQuantity)
        {
            if (saleQuantity > StockQuantity) return false;
            
            StockQuantity -= saleQuantity;
            ModifyItem();
            
            var transaction = new SaleTransaction(Id, saleQuantity, UnitPrice);
            _salesHistory.Push(transaction);
            
            return true;
        }

        public static bool ReverseLastSale()
        {
            if (_salesHistory.Count == 0) return false;
            
            var lastSale = _salesHistory.Pop();
            var item = FindItems(itemId: lastSale.ItemId).FirstOrDefault();
            
            if (item.Id != 0)
            {
                item.StockQuantity += lastSale.Quantity;
                item.ModifyItem();
            }
            
            return true;
        }

        public static List<InventoryItem> FindItems(uint? itemId = null, 
                                                  string searchTerm = null, 
                                                  ProductCategory? categoryFilter = null)
        {
            return _inventory.Where(item =>
                (!itemId.HasValue || item.Id == itemId) &&
                (string.IsNullOrEmpty(searchTerm) || item.Name.Contains(searchTerm)) &&
                (!categoryFilter.HasValue || item.Category == categoryFilter)
            ).ToList();
        }

        public override string ToString()
        {
            return $"{Id,-4} {UnitPrice,-8:C} {StockQuantity,-6} " +
                   $"{Category,-12} {(IsAvailableInWarehouse ? "Yes" : "No"),-8} {Name}";
        }

        public static string GetInventoryHeader()
        {
            return $"{"ID",-4} {"Price",-8} {"Qty",-6} {"Category",-12} {"In Stock",-8} Name\n";
        }

        public static string DisplayAllItems()
        {
            return GetInventoryHeader() + string.Join("\n", _inventory.Select(item => item.ToString()));
        }

        public static decimal CalculateTotalSales()
        {
            return _salesHistory.Sum(sale => sale.Quantity * sale.PicePerUnit);
        }

        public static void DisplaySalesReport()
        {
            Console.WriteLine($"{"Item ID",-8} {"Qty",-6} {"Price",-8}");
            foreach (var sale in _salesHistory)
            {
                Console.WriteLine($"{sale.ItemId,-8} {sale.Quantity,-6} {sale.PicePerUnit,-8:C}");
            }
            Console.WriteLine($"TOTAL: {CalculateTotalSales():C}");
        }
    }

    public struct SaleTransaction
    {
        public uint ItemId { get; }
        public uint Quantity { get; }
        public decimal PicePerUnit { get; }

        public SaleTransaction(uint itemId, uint quantity, decimal price)
        {
            ItemId = itemId;
            Quantity = quantity;
            PicePerUnit = price;
        }
    }

    class InventoryManager
    {
        static uint GetUnsignedIntegerInput(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                if (uint.TryParse(Console.ReadLine(), out uint result))
                    return result;
                
                Console.WriteLine("Please enter a valid positive number.");
            }
        }

        static void InitializeSampleData()
        {
            new InventoryItem("Halva", 120m, 50, true, ProductCategory.Food).RegisterItem();
            new InventoryItem("Chewing Gum", 1m, 150, true, ProductCategory.Food).RegisterItem();
            new InventoryItem("Bun", 30m, 30, true, ProductCategory.Food).RegisterItem();
            new InventoryItem("Soda", 70m, 20, true, ProductCategory.Beverages).RegisterItem();
            new InventoryItem("Diploma", 30000m, 1600, false, ProductCategory.Stationery).RegisterItem();
        }

        static void Main()
        {
            InitializeSampleData();
            
            while (true)
            {
                Console.WriteLine("\n=== Inventory Management System ===");
                Console.WriteLine("1. Add new item");
                Console.WriteLine("2. Remove item");
                Console.WriteLine("3. Restock item");
                Console.WriteLine("4. Sell item");
                Console.WriteLine("5. Search inventory");
                Console.WriteLine("6. Undo last sale");
                Console.WriteLine("7. Sales report");
                Console.WriteLine("0. Exit");
                
                var choice = GetUnsignedIntegerInput("Select option:");

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        AddNewItem();
                        break;
                    case 2:
                    case 3:
                    case 4:
                        ProcessItemOperation(choice);
                        break;
                    case 5:
                        SearchInventory();
                        break;
                    case 6:
                        UndoLastSale();
                        break;
                    case 7:
                        InventoryItem.DisplaySalesReport();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddNewItem()
        {
            try
            {
                Console.WriteLine("Enter product name:");
                var name = Console.ReadLine();
                
                Console.WriteLine("Enter price:");
                var price = decimal.Parse(Console.ReadLine());
                
                var quantity = GetUnsignedIntegerInput("Enter quantity:");
                
                Console.WriteLine("Is this item available in warehouse? (yes/no):");
                var inWarehouse = Console.ReadLine().ToLower() == "yes";
                
                Console.WriteLine("Select category:\n1. Food\n2. Beverages\n3. Stationery\n4. Electronics");
                var categoryChoice = GetUnsignedIntegerInput("Enter category number:");
                
                if (categoryChoice < 1 || categoryChoice > 4)
                {
                    Console.WriteLine("Invalid category selection.");
                    return;
                }

                var category = (ProductCategory)(categoryChoice - 1);
                var newItem = new InventoryItem(name, price, quantity, inWarehouse, category);
                
                newItem.RegisterItem();
                Console.WriteLine("Item added successfully!");
                Console.WriteLine(InventoryItem.GetInventoryHeader() + newItem.ToString());
            }
            catch
            {
                Console.WriteLine("Error adding item. Please check your inputs.");
            }
        }

        static void ProcessItemOperation(int operationType)
        {
            var itemId = GetUnsignedIntegerInput("Enter item ID:");
            var items = InventoryItem.FindItems(itemId);
            
            if (items.Count == 0)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            var item = items[0];
            
            switch (operationType)
            {
                case 2:
                    item.RemoveFromInventory();
                    Console.WriteLine("Item removed successfully.");
                    break;
                
                case 3:
                    var restockQty = GetUnsignedIntegerInput("Enter restock quantity:");
                    if (item.Restock(restockQty))
                        Console.WriteLine("Item restocked successfully.");
                    else
                        Console.WriteLine("Cannot restock - item not available in warehouse.");
                    break;
                
                case 4:
                    var saleQty = GetUnsignedIntegerInput("Enter sale quantity:");
                    if (item.ProcessSale(saleQty))
                        Console.WriteLine("Sale processed successfully.");
                    else
                        Console.WriteLine("Insufficient stock for this sale.");
                    break;
            }
        }

        static void SearchInventory()
        {
            Console.WriteLine("Enter item ID (or press Enter to skip):");
            uint? itemId = uint.TryParse(Console.ReadLine(), out uint id) ? id : null;

            Console.WriteLine("Enter category (1-4) or press Enter to skip:");
            ProductCategory? category = uint.TryParse(Console.ReadLine(), out uint cat) && cat >= 1 && cat <= 4
                ? (ProductCategory)(cat - 1)
                : null;

            Console.WriteLine("Enter search term or press Enter to skip:");
            var searchTerm = Console.ReadLine();

            var results = InventoryItem.FindItems(itemId, searchTerm, category);
            
            Console.WriteLine(InventoryItem.GetInventoryHeader());
            foreach (var item in results)
            {
                Console.WriteLine(item.ToString());
            }
        }

        static void UndoLastSale()
        {
            if (InventoryItem.ReverseLastSale())
                Console.WriteLine("Last sale reversed successfully.");
            else
                Console.WriteLine("No sales to reverse.");
        }
    }
}
