using System;
using System.Data;
using MySql.Data.MySqlClient;
using Wandala.Database.Interfaces;
using Wandala.Database.Adapter;

namespace Wandala.Database
{
    public class DatabaseConnection : IDatabaseClient, IDisposable
    {
        private readonly IQueryAdapter queryAdapter;
        private readonly MySqlConnection connection;

        public DatabaseConnection(string connectionStr)
        {
            connection = new MySqlConnection(connectionStr);
            queryAdapter = new NormalQueryReactor(this);
        }

        public void connect()
        {
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void disconnect()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public IQueryAdapter GetQueryReactor()
        {
            return queryAdapter;
        }

        public void reportDone()
        {
            Dispose();
        }

        public MySqlCommand createNewCommand()
        {
            return connection.CreateCommand();
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}