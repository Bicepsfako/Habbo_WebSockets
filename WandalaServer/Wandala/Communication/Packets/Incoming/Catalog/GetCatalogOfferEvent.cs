using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Catalog;
using Wandala.Communication.Packets.Outgoing.Catalog;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!WandalaEnvironment.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = WandalaEnvironment.GetGame().GetCatalog().ItemOffers[OfferId];

            CatalogPage Page;
            if (!WandalaEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendPacket(new CatalogOfferComposer(Item));
        }
    }
}
