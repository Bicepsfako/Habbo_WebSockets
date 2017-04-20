using Alchemy;
using Alchemy.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        MySqlCommand command = connection.CreateCommand();
        // Online users list
        public static readonly ConcurrentDictionary<User, bool> OnlineUsers = new ConcurrentDictionary<User, bool>();
        static void Main(string[] args)
        {
            Console.Title = "Wandala Server v0.0.1 | Port: " + port;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                                     __  ");
            Console.WriteLine(" \\            /    /\\      |\\   |   |  \\      /\\      |          /\\");
            Console.WriteLine("  \\    /\\    /    /  \\     | \\  |   |   \\    /  \\     |         /  \\");
            Console.WriteLine("   \\  /  \\  /    /____\\    |  \\ |   |   /   /____\\    |        /____\\");
            Console.WriteLine("    \\/    \\/    /      \\   |   \\|   |__/   /      \\   |____   /      \\   v0.0.1");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nUsing Alchemy Websockets - v2.0.0");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[Programmer] xSmoking");
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
            var client = new User(context);
            OnlineUsers.TryAdd(client, false);
        }

        private static void OnReceive(UserContext context)
        {
            Console.WriteLine("Received data from: " + context.ClientAddress);
            User client = new User(context);
            string data = context.DataFrame.ToString();
            Console.WriteLine("Data: " + data);
        }

        private static void OnSend(UserContext context)
        {
            var client = new User(context);
        }

        private static void OnDisconnect(UserContext context)
        {
            var client = new User(context);
            bool outBool;
            OnlineUsers.TryRemove(client, out outBool);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Disconnected: " + context.ClientAddress.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
