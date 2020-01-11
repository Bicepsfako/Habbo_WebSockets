using Alchemy.Classes;
using HabboWS.HabboHotel.Users;

namespace HabboWS.HabboHotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        private Habbo _habbo;
        private UserContext _userContext;

        public GameClient(int ClientId, UserContext UserContext)
        {
            this._id = ClientId;
            this._habbo = HabboEnvironment.GetOnlineUser(ClientId);
            this._userContext = UserContext;
        }

        public void SendNotification(string Message)
        {

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
