using System;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.RCON.Commands.User
{
    class AlertUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to alert a user."; }
        }

        public string Parameters
        {
            get { return "%userId% %message%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the message
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string message = Convert.ToString(parameters[1]);

            client.SendPacket(new BroadcastMessageAlertComposer(message));
            return true;
        }
    }
}