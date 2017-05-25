using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.Database.Interfaces;
using Wandala.HabboHotel.GameClients;

namespace Wandala.Communication.RCON.Commands.User
{
    class ReloadUserRankCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to reload a users rank and permissions."; }
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

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `rank` FROM `users` WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("userId", userId);
                client.GetHabbo().Rank = dbClient.GetInteger();
            }

            client.GetHabbo().GetPermissions().Init(client.GetHabbo());

            if (client.GetHabbo().GetPermissions().HasRight("mod_tickets"))
            {
                client.SendPacket(new ModeratorInitComposer(
                  WandalaEnvironment.GetGame().GetModerationManager().UserMessagePresets,
                  WandalaEnvironment.GetGame().GetModerationManager().RoomMessagePresets,
                  WandalaEnvironment.GetGame().GetModerationManager().GetTickets));
            }
            return true;
        }
    }
}