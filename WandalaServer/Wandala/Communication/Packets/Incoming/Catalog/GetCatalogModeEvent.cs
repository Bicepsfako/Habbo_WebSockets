using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Catalog;
using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.Communication.Packets.Outgoing.BuildersClub;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    class GetCatalogModeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string PageMode = Packet.PopString();
        }
    }
}
