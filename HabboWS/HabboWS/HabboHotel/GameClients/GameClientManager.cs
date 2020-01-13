using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabboWS.HabboHotel.GameClients
{
    class GameClientManager
    {
        private static readonly ILog log = LogManager.GetLogger("Wandala.HabboHotel.GameClients.GameClientManager");

        private ConcurrentDictionary<int, GameClient> _clientsById;
        private ConcurrentDictionary<string, GameClient> _clientsByUsername;

        public GameClientManager()
        {
            this._clientsById = new ConcurrentDictionary<int, GameClient>();
            this._clientsByUsername = new ConcurrentDictionary<string, GameClient>();
        }

        public GameClient GetClientByUserID(int userID)
        {
            if (_clientsById.ContainsKey(userID))
                return _clientsById[userID];
            return null;
        }

        public GameClient GetClientByUsername(string username)
        {
            if (_clientsByUsername.ContainsKey(username.ToLower()))
                return _clientsByUsername[username.ToLower()];
            return null;
        }
    }
}
