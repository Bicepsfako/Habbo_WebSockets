using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Engine
{
    class FurnitureAliasesComposer : ServerPacket
    {
        public FurnitureAliasesComposer()
            : base(ServerPacketHeader.FurnitureAliasesMessageComposer)
        {
            base.WriteInteger(0);          
        }
    }
}
