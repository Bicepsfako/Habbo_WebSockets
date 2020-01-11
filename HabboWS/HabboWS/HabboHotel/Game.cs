using log4net;
using HabboWS.HabboHotel.Rooms;

namespace HabboWS.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.HabboHotel.Game");

        private readonly RoomManager _roomManager;

        public Game()
        {
            this._roomManager = new RoomManager();
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }
    }
}
