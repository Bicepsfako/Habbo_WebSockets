using log4net;
using HabboWS.HabboHotel.Rooms;
using HabboWS.HabboHotel.GameClients;

namespace HabboWS.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.HabboHotel.Game");

        private readonly GameClientManager _gameClientManager;
        private readonly RoomManager _roomManager;

        public Game()
        {
            this._gameClientManager = new GameClientManager();
            this._roomManager = new RoomManager();
        }

        public GameClientManager GetGameClientManager()
        {
            return _gameClientManager;
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }
    }
}
