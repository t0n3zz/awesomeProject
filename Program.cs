using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== УЧЕТ РАСХОДОВ ===");
        int count = AskCount();
        
        string[] names = new string[count];
        decimal[] money = new decimal[count];
        
        FillExpenses(names, money, count);
        
        ShowMenu(names, money);
    }
    
    static int AskCount()
    {
        int count = 0;
        while (true)
        {
            Console.Write("сколько операций? (2-40, не больше не меньше): ");
            string input = Console.ReadLine();
            
            if (int.TryParse(input, out count) && count >= 2 && count <= 40)
            {
                return count;
            }
            Console.WriteLine("ошибка! только от 2 до 40");
        }
    }
    
    static void FillExpenses(string[] names, decimal[] money, int count)
    {
        Console.WriteLine("\nвводите траты как: Название; Сумма");
        Console.WriteLine("пример: Хлеб; 50");
        
        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                Console.Write($"{i+1}. ");
                string input = Console.ReadLine();
                
                string[] parts = input.Split(';');
                
                if (parts.Length == 2)
                {
                    string name = parts[0].Trim();
                    string sumStr = parts[1].Trim();
                    
                    if (name != "" && decimal.TryParse(sumStr, out decimal sum) && sum > 0)
                    {
                        names[i] = name;
                        money[i] = sum;
                        break;
                    }
                }
                Console.WriteLine("неа");
            }
        }
    }
    
    static void ShowMenu(string[] names, decimal[] money)
    {
        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1 - Показать все траты");
            Console.WriteLine("2 - Статистика");
            Console.WriteLine("3 - Сортировка по цене");
            Console.WriteLine("4 - Конвертация валюты");
            Console.WriteLine("5 - Поиск по названию");
            Console.WriteLine("0 - Выход");
            Console.Write("выберите свое подходящее обязательно пожалуйста: ");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                ShowAll(names, money);
            }
            else if (choice == "2")
            {
                ShowStats(money);
            }
            else if (choice == "3")
            {
                SortByPrice(names, money);
            }
            else if (choice == "4")
            {
                ConvertMoney(money);
            }
            else if (choice == "5")
            {
                SearchName(names, money);
            }
            else if (choice == "0")
            {
                Console.WriteLine("выход.");
                break;
            }
            else
            {
                Console.WriteLine("неверный выбор!");
            }
        }
    }
    
    static void ShowAll(string[] names, decimal[] money)
    {
        Console.WriteLine("\n=== ВСЕ ТРАТЫ ===");
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{i+1}. {names[i]} - {money[i]} руб.");
        }
    }
    
    static void ShowStats(decimal[] money)
    {
        decimal total = 0;
        decimal max = money[0];
        decimal min = money[0];
        
        for (int i = 0; i < money.Length; i++)
        {
            total += money[i];
            if (money[i] > max) max = money[i];
            if (money[i] < min) min = money[i];
        }
        
        decimal average = total / money.Length;
        
        Console.WriteLine("\n=== СТАТИСТИКА ===");
        Console.WriteLine($"всего потрачено: {total} руб.");
        Console.WriteLine($"вредняя трата: {average} руб.");
        Console.WriteLine($"самая большая трата: {max} руб.");
        Console.WriteLine($"самая маленькая трата: {min} руб.");
    }
    
    static void SortByPrice(string[] names, decimal[] money)
    {
        for (int i = 0; i < money.Length - 1; i++)
        {
            for (int j = 0; j < money.Length - 1; j++)
            {
                if (money[j] > money[j + 1])
                {
                    decimal tempMoney = money[j];
                    money[j] = money[j + 1];
                    money[j + 1] = tempMoney;
                    
                    string tempName = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = tempName;
                }
            }
        }
        Console.WriteLine("отсортировано по цене");
    }
    
    static void ConvertMoney(decimal[] money)
    {
        Console.WriteLine("\nвыберите валюту:");
        Console.WriteLine("1 - Доллары ($)");
        Console.WriteLine("2 - Евро (€)");
        Console.WriteLine("3 - Свой курс");
        Console.Write("Ваш выбор: ");
        
        string choice = Console.ReadLine();
        decimal kurs = 0;
        string valuta = "";
        
        if (choice == "1")
        {
            kurs = 0.011m;
            valuta = "$";
        }
        else if (choice == "2")
        {
            kurs = 0.010m;
            valuta = "€";
        }
        else if (choice == "3")
        {
            Console.Write("введите курс (сколько рублей за 1 валюту): ");
            string input = Console.ReadLine();
            if (decimal.TryParse(input, out kurs) && kurs > 0)
            {
                Console.Write("введите символ валюты: ");
                valuta = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("неверный курс!");
                return;
            }
        }
        else
        {
            Console.WriteLine("неверный выбор!");
            return;
        }
        
        Console.WriteLine($"\n=== В {valuta} ===");
        for (int i = 0; i < money.Length; i++)
        {
            decimal converted = money[i] * kurs;
            Console.WriteLine($"{i+1}. {converted} {valuta}");
        }
    }
    
    static void SearchName(string[] names, decimal[] money)
    {
        Console.Write("что ищем?");
        string search = Console.ReadLine().ToLower();
        
        Console.WriteLine("=== НАЙДЕНО ===");
        bool found = false;
        
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].ToLower().Contains(search))
            {
                Console.WriteLine($"{names[i]} - {money[i]} руб.");
                found = true;
            }
        }
        
        if (!found)
        {
            Console.WriteLine("ничего не найдено");
        }
    }
}
