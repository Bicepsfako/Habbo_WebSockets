using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Wandala.Communication.Packets.Outgoing.Marketplace
{
    class MarketplaceCancelOfferResultComposer : ServerPacket
    {
        public MarketplaceCancelOfferResultComposer(int OfferId, bool Success)
            : base(ServerPacketHeader.MarketplaceCancelOfferResultMessageComposer)
        {
            base.WriteInteger(OfferId);
            base.WriteBoolean(Success);
        }
    }
}
