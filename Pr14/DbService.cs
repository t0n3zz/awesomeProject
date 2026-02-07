using Microsoft.Data.Sqlite;
using Pr14.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pr14
{
    public static class DbService
    {
        private const string ConnString = "Data Source=shop.db";

        public static void Initialize()
        {
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Films (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PreviewFilePath TEXT,
                        Title TEXT,
                        Rating REAL,
                        StartDate TEXT, 
                        AgeRating TEXT,
                        ShortDescription TEXT,
                        Genres TEXT,
                        PriceForBasic DECIMAL,
                        PriceForVip DECIMAL
                    );

                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Login TEXT UNIQUE,
                        Password TEXT,
                        FullName TEXT,
                        Email TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Sessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FilmId INTEGER,
                        SessionTime TEXT,
                        HallName TEXT, 
                        HallType TEXT,
                        FOREIGN KEY (FilmId) REFERENCES Films(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Tickets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER,
                        SessionId INTEGER,
                        SeatNumber INTEGER,
                        Price DECIMAL,
                        FOREIGN KEY (UserId) REFERENCES Users(Id),
                        FOREIGN KEY (SessionId) REFERENCES Sessions(Id)
                    );
                ";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    INSERT OR IGNORE INTO Films (Id, Title, Rating, StartDate, AgeRating, Genres, PriceForBasic, PriceForVip, ShortDescription, PreviewFilePath) 
                    VALUES 
                    (1, 'Интерстеллар', 8.6, '2026-02-10', '12+', 'Научная фантастика, Драма', 350, 600, 'Путешествие группы исследователей через черную дыру.', 'https://avatars.mds.yandex.net/i?id=274968a1aa8f3540300ead370c0b54d8_l-5452983-images-thumbs&n=13'),
                    (2, 'Начало', 8.8, '2026-02-12', '16+', 'Боевик, Фантастика', 300, 550, 'Технология внедрения в сны.', 'https://static.kion.ru/content/mts/movie/70008567/posters/HORIZONTAL_633a7630f2ef11d8f606122723cd6afa.webp'),
                    (3, 'Джентльмены', 8.5, '2026-02-15', '18+', 'Криминал, Комедия', 400, 700, 'Наркобарон пытается продать свою империю.', 'https://is1-ssl.mzstatic.com/image/thumb/Video123/v4/f2/ee/3d/f2ee3d79-47fe-199a-ff9f-984c45d5ab91/volga0550gentlemen_poster_ru_16x9.jpg/1200x675.jpg');

                    INSERT OR IGNORE INTO Sessions (Id, FilmId, SessionTime, HallName, HallType) 
                    VALUES 
                    (1, 1, '12:00', 'Зал 1', 'Classic'),
                    (2, 1, '18:00', 'Зал 3 (VIP)', 'VIP'),
                    (3, 2, '14:00', 'Зал 2', 'Classic'),
                    (4, 3, '21:00', 'Зал 3 (VIP)', 'VIP');

                    INSERT OR IGNORE INTO Users (Id, Login, Password, FullName, Email) 
                    VALUES 
                    (1, 'admin', '123', 'Иванов Иван Иванович', 'admin@cinema.ru');
                ";
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Pr14.Models.Film> GetFilms()
        {
            var films = new List<Pr14.Models.Film>();

            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Films";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        films.Add(new Pr14.Models.Film
                        {
                            Id = reader.GetInt32(0),
                            PreviewFilePath = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            Title = reader.IsDBNull(2) ? "Без названия" : reader.GetString(2),
                            Rating = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                            StartDate = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            AgeRating = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            ShortDescription = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            Genres = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            PriceForBasic = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            PriceForVip = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9)
                        });
                    }
                }
            }
            return films;
        }

        public static User GetUser(string login, string password)
        {
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE Login = @log AND Password = @pass";
                cmd.Parameters.AddWithValue("@log", login);
                cmd.Parameters.AddWithValue("@pass", password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            FullName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        };
                    }
                }
            }
            return null;
        }

        public static bool RegisterUser(User user)
        {
            try
            {
                using (var connection = new SqliteConnection(ConnString))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO Users (Login, Password, FullName, Email) VALUES (@log, @pass, @name, @mail)";
                    cmd.Parameters.AddWithValue("@log", user.Login);
                    cmd.Parameters.AddWithValue("@pass", user.Password);
                    cmd.Parameters.AddWithValue("@name", user.FullName);
                    cmd.Parameters.AddWithValue("@mail", user.Email);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch { return false; }
        }

        public static List<Session> GetSessionsForFilm(int filmId)
        {
            var sessions = new List<Session>();
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Sessions WHERE FilmId = @id";
                cmd.Parameters.AddWithValue("@id", filmId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessions.Add(new Session
                        {
                            Id = reader.GetInt32(0),
                            FilmId = reader.GetInt32(1),
                            SessionTime = reader.GetString(2),
                            HallName = reader.GetString(3),
                            HallType = reader.GetString(4)
                        });
                    }
                }
            }
            return sessions;
        }

        public static List<int> GetOccupiedSeats(int sessionId)
        {
            var seats = new List<int>();
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT SeatNumber FROM Tickets WHERE SessionId = @sid";
                cmd.Parameters.AddWithValue("@sid", sessionId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) seats.Add(reader.GetInt32(0));
                }
            }
            return seats;
        }

        public static void BuyTicket(int userId, int sessionId, int seat, decimal price)
        {
            using (var connection = new SqliteConnection(ConnString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Tickets (UserId, SessionId, SeatNumber, Price) VALUES (@uid, @sid, @seat, @p)";
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@sid", sessionId);
                cmd.Parameters.AddWithValue("@seat", seat);
                cmd.Parameters.AddWithValue("@p", price);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
