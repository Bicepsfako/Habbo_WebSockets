using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Core;
using Wandala.HabboHotel.Rooms;

using Wandala.Communication.Packets.Outgoing.Rooms.Permissions;
using Wandala.Communication.Packets.Outgoing.Rooms.Settings;
using Wandala.HabboHotel.Users;

using Wandala.Database.Interfaces;
using Wandala.HabboHotel.Cache;
using Wandala.HabboHotel.Cache.Type;

namespace Wandala.Communication.Packets.Incoming.Rooms.Action
{
    class AssignRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int UserId = Packet.PopInt();

            Room Room = null;
            if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (Room.UsersWithRights.Contains(UserId))
            {
                Session.SendNotification(WandalaEnvironment.GetLanguageManager().TryGetValue("room.rights.user.has_rights"));
                return;
            }

            Room.UsersWithRights.Add(UserId);

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("INSERT INTO `room_rights` (`room_id`,`user_id`) VALUES ('" + Room.RoomId + "','" + UserId + "')");
            }

            RoomUser RoomUser = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
            if (RoomUser != null && !RoomUser.IsBot)
            {
                RoomUser.SetStatus("flatctrl 1", "");
                RoomUser.UpdateNeeded = true;
                if (RoomUser.GetClient() != null)
                    RoomUser.GetClient().SendPacket(new YouAreControllerComposer(1));

                Session.SendPacket(new FlatControllerAddedComposer(Room.RoomId, RoomUser.GetClient().GetHabbo().Id, RoomUser.GetClient().GetHabbo().Username));
            }
            else
            {
                UserCache User =  WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(UserId);
                if (User != null)
                    Session.SendPacket(new FlatControllerAddedComposer(Room.RoomId, User.Id, User.Username));
            }
        }
    }
}
