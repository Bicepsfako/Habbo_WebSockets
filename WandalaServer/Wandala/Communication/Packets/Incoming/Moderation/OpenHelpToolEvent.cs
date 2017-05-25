using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Incoming;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class OpenHelpToolEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new OpenHelpToolComposer());
        }
    }
}
