using System;
using System.Threading;
using log4net;
using Wandala.Database.Interfaces;

namespace Wandala.Core
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Wandala.Core.ServerUpdater");
        private const int UPDATE_IN_SECS = 30;
        private Timer _timer;
        
        public ServerStatusUpdater()
        {
        }

        public void Init()
        {
            _timer = new Timer(new TimerCallback(OnTick), null, TimeSpan.FromSeconds(UPDATE_IN_SECS), TimeSpan.FromSeconds(UPDATE_IN_SECS));
            Console.Title = "Wandala Server | Port: 0 | Online: 0 | Rooms Loaded: 0 | Uptime: 0 day(s) 0 hour(s)";
            log.Info("Server Status Updater has been started.");
        }

        public void OnTick(object Obj)
        {
            UpdateOnlineUsers();
        }

        private void UpdateOnlineUsers()
        {
            TimeSpan Uptime = DateTime.Now - WandalaEnvironment.ServerStarted;

            int UsersOnline = WandalaEnvironment.GetGame().GetClientManager().Count;
            int RoomCount = WandalaEnvironment.GetGame().GetRoomManager().Count;

            Console.Title = "Wandala Server | Port: " + WandalaEnvironment.GetConfig().data["game.tcp.port"] + " | Online: " + UsersOnline + " | Rooms Loaded: " + RoomCount + " | Uptime: " + Uptime.Days + " day(s) " + Uptime.Hours + " hour(s)";

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `server_status` SET `users_online` = @users, `loaded_rooms` = @loadedRooms LIMIT 1;");
                dbClient.AddParameter("users", UsersOnline);
                dbClient.AddParameter("loadedRooms", RoomCount);
                dbClient.RunQuery();
            }
        }

        public void Dispose()
        {
            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            }
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
