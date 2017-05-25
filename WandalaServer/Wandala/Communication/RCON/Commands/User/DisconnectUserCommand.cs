using Wandala.HabboHotel.GameClients;

namespace Wandala.Communication.RCON.Commands.User
{
    class DisconnectUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to disconnect a user."; }
        }

        public string Parameters
        {
            get { return "%userId%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            client.Disconnect();
            return true;
        }
    }
}
