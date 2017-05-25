using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Outgoing.Catalog
{
    class GetCatalogRoomPromotionComposer : ServerPacket
    {
        public GetCatalogRoomPromotionComposer(List<RoomData> UsersRooms)
            : base(ServerPacketHeader.PromotableRoomsMessageComposer)
        {
            base.WriteBoolean(true);//wat
            base.WriteInteger(UsersRooms.Count);//Count of rooms?
            foreach (RoomData Room in UsersRooms)
            {
                base.WriteInteger(Room.Id);
               base.WriteString(Room.Name);
                base.WriteBoolean(true);
            }
        }
    }
}
