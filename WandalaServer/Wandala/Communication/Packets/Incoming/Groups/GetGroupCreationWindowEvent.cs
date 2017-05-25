using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Groups;
using Wandala.Communication.Packets.Outgoing.Groups;

namespace Wandala.Communication.Packets.Incoming.Groups
{
    class GetGroupCreationWindowEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            List<RoomData> ValidRooms = new List<RoomData>();
            foreach (RoomData Data in Session.GetHabbo().UsersRooms)
            {
                if (Data.Group == null)
                    ValidRooms.Add(Data);
            }

            Session.SendPacket(new GroupCreationWindowComposer(ValidRooms));
        }
    }
}
