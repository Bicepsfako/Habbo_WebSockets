using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class GetModeratorRoomInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int RoomId = Packet.PopInt();

            RoomData Data = WandalaEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
                return;

            Room Room;

            if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room))
                return;

            Session.SendPacket(new ModeratorRoomInfoComposer(Data, (Room.GetRoomUserManager().GetRoomUserByHabbo(Data.OwnerName) != null)));
        }
    }
}
