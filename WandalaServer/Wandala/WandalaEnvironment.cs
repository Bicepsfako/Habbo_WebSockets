using Alchemy;
using Alchemy.Classes;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Wandala
{
    class WandalaEnvironment
    {
        // Port setup
        static public int port = 4530;
        // MySQL Configuration
        static string connString = "Server=localhost;Database=habbo;Uid=root;Pwd=2296agosto";
        static MySqlConnection connection = new MySqlConnection(connString);
        static MySqlCommand command = connection.CreateCommand();
        // Online users list
        public static readonly Dictionary<UserContext, int> OnlineUsers = new Dictionary<UserContext, int>(); // user, user_id
        public static readonly Dictionary<int, UserContext> UsersInRoom = new Dictionary<int, UserContext>(); // room_id, user

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });

        public static void Initialize()
        {
            Console.Title = "Wandala Server v0.0.1 | Port: " + port + " | Online: 0 | Rooms Loaded: 0";
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                                     __  ");
            Console.WriteLine(" \\            /    /\\      |\\   |   |  \\      /\\      |          /\\");
            Console.WriteLine("  \\    /\\    /    /  \\     | \\  |   |   \\    /  \\     |         /  \\");
            Console.WriteLine("   \\  /  \\  /    /____\\    |  \\ |   |   /   /____\\    |        /____\\");
            Console.WriteLine("    \\/    \\/    /      \\   |   \\|   |__/   /      \\   |____   /      \\   v0.0.1");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nUsing Alchemy Websockets - v2.0.0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Initializing server on port ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(port);

            var aServer = new WebSocketServer(port, IPAddress.Any)
            {
                OnReceive = context => OnReceive(context),
                OnSend = context => OnSend(context),
                OnConnected = context => OnConnect(context),
                OnDisconnect = context => OnDisconnect(context),
                TimeOut = new TimeSpan(0, 0, 60),
                FlashAccessPolicyEnabled = true
            };

            aServer.Start();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Server initialized!");
            Console.WriteLine("Waiting for clients...");
        }

        private static void OnConnect(UserContext context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connected: " + context.ClientAddress);
            Console.ForegroundColor = ConsoleColor.White;
            OnlineUsers.Add(context, 0);
            Console.Title = "Wandala Server v0.0.1 | Port: " + port + " | Online: " + OnlineUsers.Count + " | Rooms Loaded: 0";
        }

        private static void OnReceive(UserContext context)
        {
            string data = context.DataFrame.ToString();
            string[] words = data.Split('|');
            string key = null, value = null;
            int count = 0;
            foreach (string word in words)
            {
                if (0 == count) key = word;
                else if (1 == count) value = word;
                ++count;
            }
            switch (key)
            {
                case "new_connection": // new_connection|user_id
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "UPDATE `users` SET online = '1' WHERE id = '" + value + "';";
                            command.ExecuteNonQuery();
                            OnlineUsers[context] = int.Parse(value);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                        }
                    }
                    break;
                case "get_all_rooms": // get_all_rooms
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "SELECT `id`,`roomtype`,`caption`,`users_now`,`users_max`,`password` FROM rooms ORDER BY users_now DESC LIMIT 20";
                            int dictCount = 0;
                            Dictionary<int, Dictionary<string, string>> results = new Dictionary<int, Dictionary<string, string>>();
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Dictionary<string, string> dict = new Dictionary<string, string>();
                                    dict["id"] = reader.GetString(0);
                                    dict["roomtype"] = reader.GetString(1);
                                    dict["caption"] = reader.GetString(2);
                                    dict["users_now"] = reader.GetString(3);
                                    dict["users_max"] = reader.GetString(4);
                                    dict["password"] = reader.GetString(5);
                                    results[dictCount] = dict;
                                    dictCount += 1;
                                }
                                reader.Close();
                            }
                            string json = JsonConvert.SerializeObject(results);
                            context.Send("load_all_rooms|" + json);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                        }
                    }
                    break;
                case "get_my_rooms": // get_my_rooms|user_id
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "SELECT `id`,`room_type`,`caption`,`users_now`,`users_max` FROM rooms WHERE id = '" + value + "' ORDER BY users_now";
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader.GetString(0));
                                }
                                reader.Close();
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                        }
                    }
                    break;
                case "load_room": // load_room|room_id
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "SELECT * FROM rooms WHERE id = '" + value + "';";
                            MySqlDataReader reader = command.ExecuteReader();
                            OnlineUsers[context] = int.Parse(value);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static void OnSend(UserContext context)
        {
        }

        private static void OnDisconnect(UserContext context)
        {
            try
            {
                int user_id;
                OnlineUsers.TryGetValue(context, out user_id);
                connection.Open();
                command.CommandText = "UPDATE `users` SET online = '0' WHERE id = '" + user_id + "'";
                command.ExecuteNonQuery();
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            OnlineUsers.Remove(context);
            Console.Title = "Wandala Server v0.0.1 | Port: " + port + " | Online: " + OnlineUsers.Count + " | Rooms Loaded: 0";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Disconnected: " + context.ClientAddress.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PerformShutDown()
        {
            Console.Clear();
            Console.WriteLine("Server shutting down...");
            Console.Title = "Wandala Server - SHUTTING DOWN!";

            //PlusEnvironment.GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(GetLanguageManager().TryGetValue("server.shutdown.message")));
            //GetGame().StopGameLoop();
            //Thread.Sleep(2500);
            //GetConnectionManager().Destroy();//Stop listening.
            //GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            //GetGame().GetPacketManager().WaitForAllToComplete();
            //GetGame().GetClientManager().CloseAll();//Close all connections
            //GetGame().GetRoomManager().Dispose();//Stop the game loop.

            //using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            //{
            //    dbClient.RunQuery("TRUNCATE `catalog_marketplace_data`");
            //    dbClient.RunQuery("UPDATE `users` SET `online` = '0', `auth_ticket` = NULL");
            //    dbClient.RunQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0'");
            //    dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            //}

            //log.Info("Plus Emulator has successfully shutdown.");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
