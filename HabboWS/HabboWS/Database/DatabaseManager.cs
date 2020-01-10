using System;
using MySql.Data.MySqlClient;
using HabboWS.Core;
using HabboWS.Database.Interfaces;

namespace HabboWS.Database
{
    public sealed class DatabaseManager
    {
        private readonly string _connectionStr;

        public DatabaseManager(string ConnectionStr)
        {
            this._connectionStr = ConnectionStr;
        }

        public bool IsConnected()
        {
            try
            {
                MySqlConnection Con = new MySqlConnection(_connectionStr);
                Con.Open();
                MySqlCommand CMD = Con.CreateCommand();
                CMD.CommandText = "SELECT 1+1";
                CMD.ExecuteNonQuery();

                CMD.Dispose();
                Con.Close();
            }
            catch (MySqlException)
            {
                return false;
            }

            return true;
        }

        public IQueryAdapter GetQueryReactor()
        {
            try
            {
                IDatabaseClient DbConnection = new DatabaseConnection(this._connectionStr);

                DbConnection.connect();

                return DbConnection.GetQueryReactor();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                return null;
            }
        }
    }
}
