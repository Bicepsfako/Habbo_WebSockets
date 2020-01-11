// Alchemy
using Alchemy;
using Alchemy.Classes;
// Habbo
using HabboWS.Core;
using HabboWS.Database;
using HabboWS.Database.Interfaces;
using HabboWS.HabboHotel.Users;
using HabboWS.HabboHotel.Users.UserData;
using HabboWS.Utilities;
using HabboWS.Communication;
// Log
using log4net;
// MySQL
using MySql.Data.MySqlClient;
// System
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using HabboWS.HabboHotel;
using HabboWS.Core.Language;

namespace HabboWS
{
    class HabboEnvironment
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.HabboEnvironment");
        public const string serverName = "Habbo WebSocket Server";
        public const string serverVersion = "0.0.1";

        private static Game _game;
        private static ConfigurationData _configuration;
        private static LanguageManager _languageManager;
        private static WebSocketServer _ws;
        private static DatabaseManager _manager;

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });
        private static ConcurrentDictionary<int, Habbo> _onlineUsers = new ConcurrentDictionary<int, Habbo>();

        public static void Initialize()
        {
            UpdateConsoleTitle();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(serverName + " v" + serverVersion);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Created by @joaocovaes");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nUsing Alchemy Websockets - v2.0.0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Loading server...\n\n");

            try
            {
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _configuration = new ConfigurationData(Path.Combine(directory, @"config.ini"));
                var connectionString = new MySqlConnectionStringBuilder
                {
                    ConnectionTimeout = 10,
                    Database = _configuration.data["db.name"],
                    DefaultCommandTimeout = 30,
                    Logging = false,
                    MaximumPoolSize = uint.Parse(_configuration.data["db.pool.maxsize"]),
                    MinimumPoolSize = uint.Parse(_configuration.data["db.pool.minsize"]),
                    Password = _configuration.data["db.password"],
                    Pooling = true,
                    Port = uint.Parse(_configuration.data["db.port"]),
                    Server = _configuration.data["db.hostname"],
                    UserID = _configuration.data["db.username"],
                    AllowZeroDateTime = true,
                    ConvertZeroDateTime = true,
                };

                _manager = new DatabaseManager(connectionString.ToString());
                if (!_manager.IsConnected())
                {
                    log.Error("Failed to connect to the specified MySQL server.");
                    Console.ReadKey(true);
                    Environment.Exit(1);
                    return;
                }
                log.Info("Connected to the database!");

                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("TRUNCATE `catalog_marketplace_data`");
                    dbClient.RunQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0';");
                    dbClient.RunQuery("UPDATE `users` SET `online` = '0' WHERE `online` = '1'");
                    dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
                }

                //Get the configuration & Game set.
                _languageManager = new LanguageManager();
                _languageManager.Init();

                _game = new Game();
                

                _ws = new WebSocketServer(int.Parse(GetConfig().data["ws.port"]), IPAddress.Any)
                {
                    OnReceive = WebSocket.OnReceive,
                    OnSend = WebSocket.OnSend,
                    OnConnected = WebSocket.OnConnect,
                    OnDisconnect = WebSocket.OnDisconnect,
                    TimeOut = new TimeSpan(0, 5, 0)
                };
                log.Info("WebSocket Open!");

                _ws.Start();
                log.Info("Server Started!");

                var command = string.Empty;
                while (command != "exit")
                {
                    command = Console.ReadLine();
                }

                _ws.Stop();
            }
            catch (KeyNotFoundException e)
            {
                log.Error("Please check your configuration file - some values appear to be missing." + e.Message);
                log.Error("Press any key to shut down ...");

                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (InvalidOperationException e)
            {
                log.Error("Failed to initialize Wandala Server: " + e.Message);
                log.Error("Press any key to shut down ...");

                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (Exception e)
            {
                log.Error("Fatal error during startup: " + e.Message);
                log.Error("Press a key to exit");

                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
        }

        public static void PerformShutDown()
        {
            Console.Clear();
            Console.WriteLine("Server shutting down...");
            Console.Title = "Wandala Server - SHUTTING DOWN!";

            log.Info(serverName + " has successfully shutdown.");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static string GetUsernameById(int UserId)
        {
            string Name = "Unknown User";

            /*GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client != null && Client.GetHabbo() != null)
                return Client.GetHabbo().Username;

            UserCache User = HabboEnvironment.GetGame().GetCacheManager().GenerateUser(UserId);
            if (User != null)
                return User.Username;*/

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM `users` WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                Name = dbClient.GetString();
            }

            if (string.IsNullOrEmpty(Name))
                Name = "Unknown User";

            return Name;
        }

        public static double GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        public static long Now()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalMilliseconds;
            return (long)unixTime;
        }

        private static bool isValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static LanguageManager GetLanguageManager()
        {
            return _languageManager;
        }

        public static DatabaseManager GetDatabaseManager()
        {
            return _manager;
        }

        public static Habbo GetOnlineUser(int Id)
        {
            return _onlineUsers[Id];
        }

        public static ICollection<Habbo> GetOnlineUsers()
        {
            return _onlineUsers.Values;
        }

        public static void AddToOnline(int Id, Habbo Data)
        {
            _onlineUsers.TryAdd(Id, Data);
        }

        public static bool RemoveFromOnline(int Id, out Habbo Data)
        {
            return _onlineUsers.TryRemove(Id, out Data);
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static string BoolToEnum(bool Bool)
        {
            return (Bool == true ? "1" : "0");
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            return RandomNumber.GenerateNewRandom(Min, Max);
        }

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!isValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        public static void UpdateConsoleTitle()
        {
            Console.Title = serverName + " " + serverVersion + " | Online: " + _onlineUsers.Count;
        }
    }
}
