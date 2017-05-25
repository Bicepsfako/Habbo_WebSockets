using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Settings
{
    class GetRoomFilterListComposer : ServerPacket
    {
        public GetRoomFilterListComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomFilterListMessageComposer)
        {
            base.WriteInteger(Instance.WordFilterList.Count);
            foreach (string Word in Instance.WordFilterList)
            {
               base.WriteString(Word);
            }
        }
    }
}
