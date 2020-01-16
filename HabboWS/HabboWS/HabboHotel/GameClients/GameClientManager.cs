using Alchemy.Classes;
using log4net;
using System.Collections.Concurrent;

namespace HabboWS.HabboHotel.GameClients
{
    public class GameClientManager
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

        public void CreateClient(int UserId, UserContext UserContext)
        {
            GameClient client = new GameClient(UserId, UserContext);
            _clientsById.TryAdd(UserId, client);
            _clientsByUsername.TryAdd(client.GetHabbo().Username, client);
        }

        public void DisposeClient(int UserId)
        {
            GameClient client = null;
            _clientsById.TryRemove(UserId, out client);
            _clientsByUsername.TryRemove(client.GetHabbo().Username, out client);
        }

        public void Dispose()
        {

        }
    }
}
