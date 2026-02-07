using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Pr13.Models;

namespace Pr13
{
    public static class DbService
    {
        private const string ConnString = "Data Source=shop.db";

        public static void Initialize()
        {
            using (var connection = new SqliteConnection(ConnString)) { 
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Price DECIMAL,
                        ImagePath TEXT
                    )";
                cmd.ExecuteNonQuery();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM Products";
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0 )
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"
                    INSERT INTO Products(Name, Price, ImagePath) VALUES
                        ('Губка', 150, 'https://ir.ozone.ru/s3/multimedia-6/6530562390.jpg'),
                        ('Принглс', 400, 'https://avatars.mds.yandex.net/get-mpic/12477477/2a00000195d568a8679336d5ec136f548e8c/orig'),
                        ('Перчатки', 100, 'https://basket-21.wbbasket.ru/vol3511/part351189/351189844/images/big/1.webp');";
                    insertCmd.ExecuteNonQuery();
                }
                var orderCmd = connection.CreateCommand();
                orderCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT,
                        Email TEXT,
                        Address TEXT,
                        TotalSum DECIMAL,
                        OrderDate TEXT
                    )";
                orderCmd.ExecuteNonQuery();
            }
        }

        public static List<Product> GetProducts()
        {
            var list = new List<Product>();
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Products";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            ImagePath = reader.GetString(3)
                        });
                    }
                }
            }
            return list;
        }
    }
}
