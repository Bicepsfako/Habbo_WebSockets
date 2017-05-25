using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class GetMarketplaceConfigurationEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new MarketplaceConfigurationComposer());
        }
    }
}