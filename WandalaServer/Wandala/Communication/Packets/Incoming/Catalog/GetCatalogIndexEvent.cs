using System;
using Wandala.Communication.Packets.Incoming;

using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.Communication.Packets.Outgoing.BuildersClub;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            /*int Sub = 0;

            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription)
            {
                Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription().SubscriptionId;
            }*/

            Session.SendPacket(new CatalogIndexComposer(Session, WandalaEnvironment.GetGame().GetCatalog().GetPages()));//, Sub));
            Session.SendPacket(new CatalogItemDiscountComposer());
            Session.SendPacket(new BCBorrowedItemsComposer());
        }
    }
}