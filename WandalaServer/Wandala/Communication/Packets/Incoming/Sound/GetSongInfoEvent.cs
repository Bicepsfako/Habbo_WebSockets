using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Wandala.Communication.Packets.Outgoing.Sound;

namespace Wandala.Communication.Packets.Incoming.Sound
{
    class GetSongInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new TraxSongInfoComposer());
        }
    }
}
