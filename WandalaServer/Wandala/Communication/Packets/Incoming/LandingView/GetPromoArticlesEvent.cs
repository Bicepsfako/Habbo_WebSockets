using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.LandingView;
using Wandala.HabboHotel.LandingView.Promotions;
using Wandala.Communication.Packets.Outgoing.LandingView;

namespace Wandala.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = WandalaEnvironment.GetGame().GetLandingManager().GetPromotionItems();

            Session.SendPacket(new PromoArticlesComposer(LandingPromotions));
        }
    }
}
