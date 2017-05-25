using System;

using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.RCON.Commands.User
{
    class GiveUserBadgeCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to give a user a badge."; }
        }

        public string Parameters
        {
            get { return "%userId% %badgeId%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the badge
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string badge = Convert.ToString(parameters[1]);

            if (!client.GetHabbo().GetBadgeComponent().HasBadge(badge))
            {
                client.GetHabbo().GetBadgeComponent().GiveBadge(badge, true, client);
                client.SendPacket(new BroadcastMessageAlertComposer("You have been given a new badge!"));
            }
            return true;
        }
    }
}