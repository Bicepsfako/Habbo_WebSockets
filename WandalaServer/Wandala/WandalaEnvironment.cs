// System
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Concurrent;

// MySQL
using MySql.Data.MySqlClient;

// Wandala
using Wandala.Core;
using Wandala.Core.FigureData;
using Wandala.Core.Language;
using Wandala.Core.Settings;
using Wandala.HabboHotel;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.Cache.Type;
using Wandala.HabboHotel.Users.UserData;
using Wandala.Utilities;
using Wandala.Database.Interfaces;
using Wandala.Database;
using Wandala.Communication.RCON;
using Wandala.Communication.ConnectionManager;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.Communication.Encryption.Keys;
using Wandala.Communication.Encryption;

// Log
using log4net;

namespace Wandala
{
    class WandalaEnvironment
    {
        private static readonly ILog log = LogManager.GetLogger("Wandala.WandalaEnvironment");
        public const string serverName = "Wandala Server";
        public const string serverBuild = "0.0.1";

        private static Encoding _defaultEncoding;
        public static CultureInfo CultureInfo;

        private static Game _game;
        private static ConfigurationData _configuration;
        private static ConnectionHandling _connectionManager;
        private static LanguageManager _languageManager;
        private static SettingsManager _settingsManager;
        private static DatabaseManager _manager;
        private static RCONSocket _rcon;
        private static FigureDataManager _figureManager;
        
        public static bool Event = false;
        public static DateTime lastEvent;
        public static DateTime ServerStarted;

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });
        private static ConcurrentDictionary<int, Habbo> _usersCached = new ConcurrentDictionary<int, Habbo>();
        public static string SWFRevision = "";

        public static void Initialize()
        {
            Console.Title = "Wandala Server v0.0.1 | Online: 0 | Rooms Loaded: 0";
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                                     __  ");
            Console.WriteLine(" \\            /    /\\      |\\   |   |  \\      /\\      |          /\\");
            Console.WriteLine("  \\    /\\    /    /  \\     | \\  |   |   \\    /  \\     |         /  \\");
            Console.WriteLine("   \\  /  \\  /    /____\\    |  \\ |   |   /   /____\\    |        /____\\");
            Console.WriteLine("    \\/    \\/    /      \\   |   \\|   |__/   /      \\   |____   /      \\");
            Console.WriteLine("");
            Console.WriteLine("                            Wandala <Build 0.0.1>");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nUsing Alchemy Websockets - v2.0.0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Loading server...");

            try
            {
                _configuration = new ConfigurationData(Path.Combine(Application.StartupPath, @"config.ini"));
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
                log.Info("Connected to Database!");

                //Reset our statistics first.
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

                _settingsManager = new SettingsManager();
                _settingsManager.Init();

                _figureManager = new FigureDataManager();
                _figureManager.Init();

                //Have our encryption ready.
                HabboEncryptionV2.Initialize(new RSAKeys());

                //Make sure RCON is connected before we allow clients to connect.
                _rcon = new RCONSocket(GetConfig().data["rcon.tcp.bindip"], int.Parse(GetConfig().data["rcon.tcp.port"]), GetConfig().data["rcon.tcp.allowedaddr"].Split(Convert.ToChar(";")));

                //Accept connections.
                _connectionManager = new ConnectionHandling(int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]), int.Parse(GetConfig().data["game.tcp.conperip"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                _connectionManager.init();

                _game = new Game();
                _game.StartGameLoop();

                Console.WriteLine();
                log.Info("Server ready on port "+ GetConfig().data["game.tcp.port"] + "! Waiting for clients...");
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
                log.Error("Fatal error during startup: " + e);
                log.Error("Press a key to exit");

                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static void PerformShutDown()
        {
            Console.Clear();
            Console.WriteLine("Server shutting down...");
            Console.Title = "Wandala Server - SHUTTING DOWN!";

            GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(GetLanguageManager().TryGetValue("server.shutdown.message")));
            GetGame().StopGameLoop();
            Thread.Sleep(2500);
            GetConnectionManager().Destroy();//Stop listening.
            GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            GetGame().GetPacketManager().WaitForAllToComplete();
            GetGame().GetClientManager().CloseAll();//Close all connections
            GetGame().GetRoomManager().Dispose();//Stop the game loop.

            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.RunQuery("TRUNCATE `catalog_marketplace_data`");
                dbClient.RunQuery("UPDATE `users` SET `online` = '0', `auth_ticket` = NULL");
                dbClient.RunQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0'");
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            }

            log.Info("Wandala Server has successfully shutdown.");

            Thread.Sleep(1000);
            Environment.Exit(0);
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

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!isValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        private static bool isValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            inputStr = inputStr.ToLower();
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!isValid(inputStr[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetUsernameById(int UserId)
        {
            string Name = "Unknown User";

            GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client != null && Client.GetHabbo() != null)
                return Client.GetHabbo().Username;

            UserCache User = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(UserId);
            if (User != null)
                return User.Username;

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM `users` WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                Name = dbClient.GetString();
            }

            if (string.IsNullOrEmpty(Name))
                Name = "Unknown User";

            return Name;
        }

        public static Habbo GetHabboById(int UserId)
        {
            try
            {
                GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Habbo User = Client.GetHabbo();
                    if (User != null && User.Id > 0)
                    {
                        if (_usersCached.ContainsKey(UserId))
                            _usersCached.TryRemove(UserId, out User);
                        return User;
                    }
                }
                else
                {
                    try
                    {
                        if (_usersCached.ContainsKey(UserId))
                            return _usersCached[UserId];
                        else
                        {
                            UserData data = UserDataFactory.GetUserData(UserId);
                            if (data != null)
                            {
                                Habbo Generated = data.user;
                                if (Generated != null)
                                {
                                    Generated.InitInformation(data);
                                    _usersCached.TryAdd(UserId, Generated);
                                    return Generated;
                                }
                            }
                        }
                    }
                    catch { return null; }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Habbo GetHabboByUsername(String UserName)
        {
            try
            {
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `users` WHERE `username` = @user LIMIT 1");
                    dbClient.AddParameter("user", UserName);
                    int id = dbClient.GetInteger();
                    if (id > 0)
                        return GetHabboById(Convert.ToInt32(id));
                }
                return null;
            }
            catch { return null; }
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static Encoding GetDefaultEncoding()
        {
            return _defaultEncoding;
        }

        public static ConnectionHandling GetConnectionManager()
        {
            return _connectionManager;
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static RCONSocket GetRCONSocket()
        {
            return _rcon;
        }

        public static FigureDataManager GetFigureManager()
        {
            return _figureManager;
        }

        public static DatabaseManager GetDatabaseManager()
        {
            return _manager;
        }

        public static LanguageManager GetLanguageManager()
        {
            return _languageManager;
        }

        public static SettingsManager GetSettingsManager()
        {
            return _settingsManager;
        }

        public static ICollection<Habbo> GetUsersCached()
        {
            return _usersCached.Values;
        }

        public static bool RemoveFromCache(int Id, out Habbo Data)
        {
            return _usersCached.TryRemove(Id, out Data);
        }
    }
}
