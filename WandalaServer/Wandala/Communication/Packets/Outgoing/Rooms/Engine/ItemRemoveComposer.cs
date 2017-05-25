using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Items;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Engine
{
    class ItemRemoveComposer : ServerPacket
    {
        public ItemRemoveComposer(Item Item, int UserId)
            : base(ServerPacketHeader.ItemRemoveMessageComposer)
        {
            base.WriteString(Item.Id.ToString());
            base.WriteBoolean(false);
            base.WriteInteger(UserId);
        }
    }
}
