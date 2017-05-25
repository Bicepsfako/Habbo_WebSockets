using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wandala.Communication.Packets.Outgoing.Sound
{
    class TraxSongInfoComposer : ServerPacket
    {
        public TraxSongInfoComposer()
            : base(ServerPacketHeader.TraxSongInfoMessageComposer)
        {
            base.WriteInteger(0);//Count
            {
  
            }
        }
    }
}
