using System;
using System.Linq;
using System.Text;

using Wandala.Communication.Packets.Outgoing.Inventory.Badges;

namespace Wandala.Communication.Packets.Incoming.Inventory.Badges
{
    class GetBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new BadgesComposer(Session));
        }
    }
}
