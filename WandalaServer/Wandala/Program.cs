using Alchemy;
using Alchemy.Classes;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Wandala
{
    class Program
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
        static void Main(string[] args)
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
                TimeOut = new TimeSpan(0, 10, 0),
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
            Console.WriteLine("Received data from: " + context.ClientAddress);
            string data = context.DataFrame.ToString();
            string[] words = data.Split('|');
            string key = null, value = null;
            int count = 0;
            foreach(string word in words)
            {
                if (0 == count) key = word;
                else if (1 == count) value = word;
                ++count;
            }
            switch (key)
            {
                case "new_connection":
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "UPDATE `users` SET online = '1' WHERE id = @VALUE;";
                            command.Parameters.AddWithValue("VALUE", value);
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
                case "load_room":
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = "SELECT * FROM rooms WHERE id = @VALUE;";
                            command.Parameters.AddWithValue("VALUE", value);
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
    }
}
