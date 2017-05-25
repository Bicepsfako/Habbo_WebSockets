using System;

using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.HabboHotel.Catalog;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogPageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PageId = Packet.PopInt();
            int Something = Packet.PopInt();
            string CataMode = Packet.PopString();

            CatalogPage Page = null;
            if (!WandalaEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

           Session.SendPacket(new CatalogPageComposer(Page, CataMode));
        }
    }
}