using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Rooms.Session;
using Wandala.Communication.Packets.Outgoing.Messenger;

namespace Wandala.Communication.Packets.Incoming.Messenger
{
    class FindNewFriendsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Instance = WandalaEnvironment.GetGame().GetRoomManager().TryGetRandomLoadedRoom();

            if (Instance != null)
            {
                Session.SendPacket(new FindFriendsProcessResultComposer(true));
                Session.SendPacket(new RoomForwardComposer(Instance.Id));
            }
            else
            {
                Session.SendPacket(new FindFriendsProcessResultComposer(false));
            }
        }
    }
}
