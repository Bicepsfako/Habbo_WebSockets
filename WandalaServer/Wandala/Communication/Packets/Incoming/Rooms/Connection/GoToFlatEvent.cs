using System;
using System.Linq;
using System.Text;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Rooms.Session;

namespace Wandala.Communication.Packets.Incoming.Rooms.Connection
{
    class GoToFlatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            if (!Session.GetHabbo().EnterRoom(Session.GetHabbo().CurrentRoom))
                Session.SendPacket(new CloseConnectionComposer());
        }
    }
}
