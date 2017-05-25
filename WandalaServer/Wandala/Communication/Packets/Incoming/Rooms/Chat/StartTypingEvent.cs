using System;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Rooms.Chat;

namespace Wandala.Communication.Packets.Incoming.Rooms.Chat
{
    public class StartTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (User == null)
                return;

            Session.GetHabbo().CurrentRoom.SendPacket(new UserTypingComposer(User.VirtualId, true));
        }
    }
}