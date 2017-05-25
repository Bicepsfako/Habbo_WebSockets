using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Wandala.Communication.Packets.Outgoing.Inventory.Trading
{
    class TradingClosedComposer : ServerPacket
    {
        public TradingClosedComposer(int UserId)
            : base(ServerPacketHeader.TradingClosedMessageComposer)
        {
            base.WriteInteger(UserId);
            base.WriteInteger(0);
        }
    }
}
