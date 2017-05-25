using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Wandala.Communication.Packets.Outgoing.Handshake
{
    class PongComposer :ServerPacket
    {
        public PongComposer()
            : base(ServerPacketHeader.PongMessageComposer)
        {

        }
    }
}
