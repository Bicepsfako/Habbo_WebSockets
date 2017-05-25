using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Help;

namespace Wandala.Communication.Packets.Incoming.Help
{
    class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            //Session.SendMessage(new SanctionStatusComposer());
        }
    }
}
