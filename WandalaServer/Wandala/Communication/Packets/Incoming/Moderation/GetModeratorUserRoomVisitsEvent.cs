using System;
using System.Data;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.Database.Interfaces;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class GetModeratorUserRoomVisitsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int UserId = Packet.PopInt();
            GameClient Target = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Target == null)
                return;

            DataTable Table = null;
            Dictionary<double, RoomData> Visits = new Dictionary<double, RoomData>();
            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `room_id`, `entry_timestamp` FROM `user_roomvisits` WHERE `user_id` = @id ORDER BY `entry_timestamp` DESC LIMIT 50");
                dbClient.AddParameter("id", UserId);
                Table = dbClient.GetTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        RoomData RData = WandalaEnvironment.GetGame().GetRoomManager().GenerateRoomData(Convert.ToInt32(Row["room_id"]));
                        if (RData == null)
                            return;

                        if (!Visits.ContainsKey(Convert.ToDouble(Row["entry_timestamp"])))
                            Visits.Add(Convert.ToDouble(Row["entry_timestamp"]), RData);
                    }
                }
            }

            Session.SendPacket(new ModeratorUserRoomVisitsComposer(Target.GetHabbo(), Visits));
        }
    }
}