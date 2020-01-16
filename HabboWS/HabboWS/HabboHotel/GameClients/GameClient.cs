using Alchemy.Classes;
using HabboWS.HabboHotel.Users;
using log4net;

namespace HabboWS.HabboHotel.GameClients
{
    public class GameClient
    {
        private static readonly ILog log = LogManager.GetLogger("Wandala.HabboHotel.GameClients.GameClient");

        private readonly int _id;
        private Habbo _habbo;
        private UserContext _userContext;

        public GameClient(int ClientId, UserContext UserContext)
        {
            this._id = ClientId;
            this._habbo = HabboEnvironment.GetOnlineUser(ClientId);
            this._userContext = UserContext;
        }

        public void SendPacket(string Message)
        {
            _userContext.Send(Message);
        }

        public UserContext GetUserContext()
        {
            return _userContext;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }
    }
}
