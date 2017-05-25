using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Users;
using Wandala.Communication.Packets.Outgoing.Rooms.Action;
using Wandala.Database.Interfaces;

namespace Wandala.Communication.Packets.Incoming.Rooms.Action
{
    class UnIgnoreUserEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().InRoom)
                return;

            Room Room = session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            string Username = packet.PopString();

            Habbo Player = WandalaEnvironment.GetHabboByUsername(Username);
            if (Player == null)
                return;

            if (!session.GetHabbo().GetIgnores().TryGet(Player.Id))
                return;

            if (session.GetHabbo().GetIgnores().TryRemove(Player.Id))
            {
                using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `user_ignores` WHERE `user_id` = @uid AND `ignore_id` = @ignoreId");
                    dbClient.AddParameter("uid", session.GetHabbo().Id);
                    dbClient.AddParameter("ignoreId", Player.Id);
                    dbClient.RunQuery();
                }

                session.SendPacket(new IgnoreStatusComposer(3, Player.Username));
            }
        }
    }
}
