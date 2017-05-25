using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.Catalog;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    class GetCatalogRoomPromotionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new GetCatalogRoomPromotionComposer(Session.GetHabbo().UsersRooms));
        }
    }
}
