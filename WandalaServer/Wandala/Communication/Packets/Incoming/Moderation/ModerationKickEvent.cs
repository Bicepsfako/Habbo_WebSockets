﻿using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class ModerationKickEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_kick"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == Session.GetHabbo().Id)
                return;

            if (Client.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotification(WandalaEnvironment.GetLanguageManager().TryGetValue("moderation.kick.disallowed"));
                return;
            }

            Room Room = null;
            if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;
            
            Room.GetRoomUserManager().RemoveUserFromRoom(Client, true, false);
        }
    }
}
