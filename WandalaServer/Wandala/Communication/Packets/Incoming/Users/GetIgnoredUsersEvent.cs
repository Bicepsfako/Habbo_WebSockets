using System.Collections.Generic;

using Wandala.HabboHotel.Users;
using Wandala.Communication.Packets.Outgoing.Users;

namespace Wandala.Communication.Packets.Incoming.Users
{
    class GetIgnoredUsersEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            List<string> ignoredUsers = new List<string>();

            foreach (int userId in new List<int>(session.GetHabbo().GetIgnores().IgnoredUserIds()))
            {
                Habbo player = WandalaEnvironment.GetHabboById(userId);
                if (player != null)
                {
                    if (!ignoredUsers.Contains(player.Username))
                        ignoredUsers.Add(player.Username);
                }
            }

            session.SendPacket(new IgnoredUsersComposer(ignoredUsers));
        }
    }
}